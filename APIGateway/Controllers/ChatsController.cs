using APIGateway.Infrastructure.Responses.Chats;
using Asp.Versioning;
using Global.Chat.Gen;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using APIGateway.Extensions.Helpers;
using Empty = Google.Protobuf.WellKnownTypes.Empty;
using APIGateway.Infrastructure.Extensions;

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
    [Route("global/history")]
    [ProducesResponseType(typeof(RestApiResponse<List<ChatMessageResponseDto>>), 200)]
    public async Task<ActionResult<RestApiResponse<List<ChatMessageResponseDto>>>> GetGlobalChatHistory()
    {
        var history = await _globalChatClient.GetHistoryAsync(new Empty());

        var response = new List<ChatMessageResponseDto>();
        foreach (var message in history.Messages)
        {
            response.Add(ChatMessageResponseDto.ChatMessageResponseFromProtoToDto(message));
        }

        return StatusCode(StatusCodes.Status200OK, RestApiResponseBuilder<List<ChatMessageResponseDto>>.Success(response));
    }

    [HttpGet]
    [Authorize]
    [Route("global")]
    public async Task WebSocketGlobalChatStream()
    {
        await WebSocketHelper.EnsureWebSocketRequestAsync(HttpContext);
        var userId = UserHelper.EnsureAuthorizedUser(HttpContext);

        using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
        lock (ActiveClients)
        {
            ActiveClients.Add(webSocket);
        }

        try
        {
            using var chatStream = _globalChatClient.ChatStream(cancellationToken: HttpContext.RequestAborted);

            var receivingTask = WebSocketHelper.ReceiveMessagesFromClient(
                webSocket,
                chatStream.RequestStream,
                HttpContext,
                userId);

            var sendingTask = WebSocketHelper.BroadcastMessagesToAllClients(
                chatStream.ResponseStream,
                ActiveClients,
                HttpContext,
                grpcResponse => ChatMessageResponseDto.ChatMessageResponseFromProtoToDto(grpcResponse));

            await Task.WhenAll(receivingTask, sendingTask);
        }
        finally
        {
            lock (ActiveClients)
            {
                ActiveClients.Remove(webSocket);
            }
            webSocket.Dispose();
        }
    }
}