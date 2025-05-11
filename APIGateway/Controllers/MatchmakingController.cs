using APIGateway.Extensions.Helpers;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using Matchmaking.Gen;
using APIGateway.Infrastructure.Responses.Matchmaking;

namespace APIGateway.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/matchmaking")]
[Produces("application/json")]
[ApiController]
public class MatchmakingController : ControllerBase
{
    private static readonly List<WebSocket> ActiveClients = new();
    private readonly ILogger<MatchmakingController> _logger;
    private readonly GameMatchmaking.GameMatchmakingClient _gameMatchmakingClient;

    public MatchmakingController(
        ILogger<MatchmakingController> logger,
        GameMatchmaking.GameMatchmakingClient gameMatchmakingClient)
    {
        _logger = logger;
        _gameMatchmakingClient = gameMatchmakingClient;
    }

    [HttpGet]
    [Authorize]
    public async Task Matchmaking()
    {
        WebSocketHelper.EnsureWebSocketRequestAsync(HttpContext);
        var userId = UserHelper.EnsureAuthorizedUser(HttpContext);

        using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
        lock (ActiveClients)
        {
            ActiveClients.Add(webSocket);
        }

        var cts = CancellationTokenSource.CreateLinkedTokenSource(HttpContext.RequestAborted);

        try
        {
            using var call = _gameMatchmakingClient.Matchmaking(
                new MatchmakingRequest { PlayerId = userId },
                cancellationToken: cts.Token
            );

            var receivingTask = WebSocketHelper.ReceiveMessagesAndDetectClose(webSocket, HttpContext);

            var sendingTask = WebSocketHelper.SendPersonalizedMessages(
                call.ResponseStream,
                webSocket,
                HttpContext,
                grpcResponse => MatchmakingResponseDto.MatchmakingResponseFromProtoToDto(grpcResponse));

            var finishedTask = await Task.WhenAny(receivingTask, sendingTask);

            if (finishedTask == receivingTask)
            {
                cts.Cancel();
            }

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