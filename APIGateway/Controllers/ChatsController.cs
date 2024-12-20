using APIGateway.Infrastructure.Requests.Chats;
using APIGateway.Infrastructure.Responses.Chats;
using Asp.Versioning;
using Common.Constants;
using Global.Chat.Gen;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text.Json;
using System.Text;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/chats")]
[Produces("application/json")]
[ApiController]
public class ChatsController : ControllerBase
{
    private static readonly List<WebSocket> ActiveClients = new();
    private readonly ILogger<ChatsController> _logger;
    private readonly GlobalChat.GlobalChatClient _globalChatClient;

    public ChatsController(
        ILogger<ChatsController> logger,
        GlobalChat.GlobalChatClient globalChatClient)
    {
        _logger = logger;
        _globalChatClient = globalChatClient;
    }

    [HttpGet]
    [Authorize]
    [Route("global")]
    public async Task GetWebSocketChatStream()
    {
        if (!HttpContext.WebSockets.IsWebSocketRequest)
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            await HttpContext.Response.WriteAsync("WebSocket connection required.");
            return;
        }

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            await HandleUnauthorizedError("Unauthorized user.");
            return;
        }

        if (!int.TryParse(userIdClaim.Value, out var userId))
        {
            await HandleUnauthorizedError("Invalid user ID.");
            return;
        }

        using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
        lock (ActiveClients)
        {
            ActiveClients.Add(webSocket);
        }

        try
        {
            using var chatStream = _globalChatClient.ChatStream();

            var receivingTask = ReceiveMessagesFromClient(webSocket, chatStream.RequestStream, userId);
            var sendingTask = BroadcastMessagesToAllClients(chatStream.ResponseStream);

            await Task.WhenAll(receivingTask, sendingTask);
        }
        catch (RpcException rpcEx)
        {
            _logger.LogError(rpcEx, "Ошибка gRPC вызова в WebSocket.");
            await HandleError(webSocket, ErrorMessages.Unavailable, WebSocketCloseStatus.InternalServerError);
        }
        catch (JsonException jsonEx)
        {
            _logger.LogError(jsonEx, "Некорректный JSON формат.");
            await HandleError(webSocket, ErrorMessages.Unavailable, WebSocketCloseStatus.InvalidPayloadData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Общая ошибка в WebSocket.");
            await HandleError(webSocket, ErrorMessages.Unavailable, WebSocketCloseStatus.InternalServerError);
        }
        finally
        {
            lock (ActiveClients)
            {
                ActiveClients.Remove(webSocket);
            }
        }
    }

    private async Task ReceiveMessagesFromClient(WebSocket webSocket, IClientStreamWriter<ChatMessageRequest> grpcRequestStream, int userId)
    {
        var buffer = new byte[1024 * 4];

        while (webSocket.State == WebSocketState.Open)
        {
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            if (result.MessageType == WebSocketMessageType.Close)
            {
                await grpcRequestStream.CompleteAsync();
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
                break;
            }

            try
            {
                var messageJson = Encoding.UTF8.GetString(buffer, 0, result.Count);
                var chatRequestDto = JsonSerializer.Deserialize<ChatMessageRequestDto>(messageJson);

                if (chatRequestDto == null || string.IsNullOrWhiteSpace(chatRequestDto.Content))
                {
                    throw new JsonException("Message content is missing or invalid.");
                }

                var grpcMessage = new ChatMessageRequest
                {
                    SenderId = userId,
                    Content = chatRequestDto.Content
                };

                await grpcRequestStream.WriteAsync(grpcMessage);
            }
            catch (JsonException ex)
            {
                _logger.LogWarning(ex, "Ошибка десериализации входящего сообщения.");
                await HandleError(webSocket, ErrorMessages.Unavailable, WebSocketCloseStatus.InvalidPayloadData);
                break;
            }
        }
    }

    private async Task BroadcastMessagesToAllClients(IAsyncStreamReader<ChatMessageResponse> grpcResponseStream)
    {
        while (await grpcResponseStream.MoveNext(CancellationToken.None))
        {
            var chatResponseDto = ChatMessageResponseDto.ChatMessageResponseFromProtoToDto(grpcResponseStream.Current);
            var messageJson = JsonSerializer.Serialize(chatResponseDto);
            var messageBuffer = Encoding.UTF8.GetBytes(messageJson);

            lock (ActiveClients)
            {
                foreach (var client in ActiveClients.ToList())
                {
                    if (client.State == WebSocketState.Open)
                    {
                        _ = SendMessageToClient(client, messageBuffer);
                    }
                }
            }
        }
    }

    private async Task SendMessageToClient(WebSocket webSocket, byte[] messageBuffer)
    {
        try
        {
            await webSocket.SendAsync(new ArraySegment<byte>(messageBuffer), WebSocketMessageType.Text, true, CancellationToken.None);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка отправки сообщения WebSocket клиенту.");
        }
    }

    private async Task HandleError(WebSocket webSocket, string errorMessage, WebSocketCloseStatus closeStatus)
    {
        try
        {
            var errorResponse = new { Error = true, Message = errorMessage };
            var errorBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(errorResponse));

            await webSocket.SendAsync(new ArraySegment<byte>(errorBytes), WebSocketMessageType.Text, true, CancellationToken.None);
            await webSocket.CloseAsync(closeStatus, errorMessage, CancellationToken.None);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка обработки WebSocket-сообщения об ошибке.");
        }
    }

    private async Task HandleUnauthorizedError(string errorMessage)
    {
        HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
        await HttpContext.Response.WriteAsJsonAsync(new { Error = true, Message = errorMessage });
    }
}
