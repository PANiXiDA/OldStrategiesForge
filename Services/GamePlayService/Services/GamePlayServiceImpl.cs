using Common.Constants;
using GameEngine.Domains;
using GamePlayService.Extensions.Helpers;
using GamePlayService.Infrastructure.Enums;
using GamePlayService.Infrastructure.Models;
using GamePlayService.Infrastructure.Requests;
using Games.Entities.Gen;
using Games.Gen;
using Sessions.Gen;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace GamePlayService.Services;

public class GamePlayServiceImpl : BackgroundService
{
    private ILogger<GamePlayServiceImpl> _logger;

    private readonly UdpClient _udpServer;
    private readonly JwtHelper _jwtHelper;

    private readonly GamesService.GamesServiceClient _gamesService;
    private readonly SessionsService.SessionsServiceClient _sessionsService;

    private static readonly ConcurrentDictionary<Guid, GameSession> _games = new();

    public GamePlayServiceImpl(
        ILogger<GamePlayServiceImpl> logger,
        JwtHelper jwtHelper,
        GamesService.GamesServiceClient gamesService,
        SessionsService.SessionsServiceClient sessionsService)
    {
        _logger = logger;

        _udpServer = new UdpClient(PortsConstants.GamePlayServicePort);
        _jwtHelper = jwtHelper;

        _gamesService = gamesService;
        _sessionsService = sessionsService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var received = await _udpServer.ReceiveAsync(stoppingToken);
            var clientEndpoint = received.RemoteEndPoint;
            var incomingMessage = Encoding.UTF8.GetString(received.Buffer);

            await HandleIncomingMessage(incomingMessage, clientEndpoint);
        }
    }

    private async Task HandleIncomingMessage(string incomingMessage, IPEndPoint clientEndpoint)
    {
        if (!JsonHelper.TryDeserialize<IncomingMessage>(incomingMessage, out var message) || message == null)
        {
            _logger.LogError($"Ошибка десериализации JSON: {incomingMessage}");
            return;
        }

        await ChooseHandleStategy(message, clientEndpoint);
    }

    private async Task ChooseHandleStategy(IncomingMessage message, IPEndPoint clientEndpoint)
    {
        switch (message.MessageType)
        {
            case MessageType.Connection:
                if (JsonHelper.TryDeserialize<ConnectionMessage>(message.Message, out var connectionMessage) && connectionMessage != null)
                {
                    //await HandleConnection(connectionMessage, clientEndpoint);
                }
                else
                {
                    _logger.LogError($"Ошибка десериализации JSON: {message}");
                }
                break;

            case MessageType.Command:
                break;

            case MessageType.Surrender:
                break;

            default:
                _logger.LogWarning($"Неизвестный тип сообщения: {message.MessageType}");
                break;
        }
    }
}