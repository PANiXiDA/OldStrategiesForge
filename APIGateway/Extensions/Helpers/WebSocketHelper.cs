using APIGateway.Infrastructure.Requests.Chats;
using Common.Constants;
using Global.Chat.Gen;
using Grpc.Core;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace APIGateway.Extensions.Helpers;

public static class WebSocketHelper
{
    public static async Task EnsureWebSocketRequestAsync(HttpContext context)
    {
        if (!context.WebSockets.IsWebSocketRequest)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync(ErrorMessages.WebSocketConnection);
            throw new InvalidOperationException(ErrorMessages.WebSocketConnection);
        }
    }

    public static async Task SendMessageToClient(WebSocket webSocket, byte[] messageBuffer)
    {
        await webSocket.SendAsync(new ArraySegment<byte>(messageBuffer), WebSocketMessageType.Text, true, CancellationToken.None);
    }

    public static async Task ReceiveMessagesFromClient<TRequest>(
        WebSocket webSocket,
        IClientStreamWriter<TRequest> grpcRequestStream,
        HttpContext context,
        int userId)
    {
        var buffer = new byte[1024 * 4];

        while (webSocket.State == WebSocketState.Open)
        {
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), context.RequestAborted);

            if (result.MessageType == WebSocketMessageType.Close)
            {
                await grpcRequestStream.CompleteAsync();
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
                break;
            }

            var messageJson = Encoding.UTF8.GetString(buffer, 0, result.Count);

            TRequest grpcMessage = typeof(TRequest) switch
            {
                Type requestType when requestType == typeof(ChatMessageRequest) =>
                    (TRequest)(object)ProcessChatMessageRequest(messageJson, userId),

                _ => throw new InvalidOperationException($"Unsupported message type: {typeof(TRequest).Name}")
            };

            await grpcRequestStream.WriteAsync(grpcMessage);
        }
    }

    private static ChatMessageRequest ProcessChatMessageRequest(string messageJson, int userId)
    {
        var chatRequestDto = JsonSerializer.Deserialize<ChatMessageRequestDto>(messageJson);

        if (chatRequestDto == null || string.IsNullOrWhiteSpace(chatRequestDto.Content))
        {
            throw new JsonException("Message content is missing or invalid.");
        }

        return new ChatMessageRequest
        {
            SenderId = userId,
            Content = chatRequestDto.Content
        };
    }

    public static async Task BroadcastMessagesToAllClients<TResponse>(
        IAsyncStreamReader<TResponse> grpcResponseStream,
        List<WebSocket> activeClients,
        HttpContext context,
        Func<TResponse, object> responseConverter)
    {
        while (await grpcResponseStream.MoveNext(context.RequestAborted))
        {
            var responseDto = responseConverter(grpcResponseStream.Current);
            var responseJson = JsonSerializer.Serialize(responseDto);
            var messageBuffer = Encoding.UTF8.GetBytes(responseJson);

            var sendTasks = new List<Task>();
            lock (activeClients)
            {
                foreach (var client in activeClients.ToList())
                {
                    if (client.State == WebSocketState.Open)
                    {
                        sendTasks.Add(SendMessageToClient(client, messageBuffer));
                    }
                }
            }
            await Task.WhenAll(sendTasks);
        }
    }
}