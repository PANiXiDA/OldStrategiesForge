using Common.Constants;
using GamePlayService.BL.BL.Interfaces;
using GamePlayService.Extensions.Helpers;
using GamePlayService.Infrastructure.Enums;
using GamePlayService.Infrastructure.Models;
using GamePlayService.Infrastructure.Requests;
using Games.Gen;
using Hangfire;
using RedLockNet;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using Tools.RabbitMQ;
using Tools.Redis;

namespace GamePlayService.Services;

public class GamePlayServiceImpl : BackgroundService
{
    private const string GameSessionKeyPrefix = "game";

    private ILogger<GamePlayServiceImpl> _logger;

    private readonly UdpClient _udpServer;
    private readonly IDistributedLockFactory _distributedLockFactory;
    private readonly IServiceProvider _serviceProvider;
    private readonly IRabbitMQClient _rabbitMQClient;
    private readonly IBackgroundJobClient _backgroundJobClient;
    private readonly IRedisCache _redisCache;

    private readonly GamesService.GamesServiceClient _gamesService;

    public GamePlayServiceImpl(
        ILogger<GamePlayServiceImpl> logger,
        IDistributedLockFactory distributedLockFactory,
        IServiceProvider serviceProvider,
        IRabbitMQClient rabbitMQClient,
        IBackgroundJobClient backgroundJobClient,
        IRedisCache redisCache,
        GamesService.GamesServiceClient gamesService)
    {
        _logger = logger;

        _udpServer = new UdpClient(PortsConstants.GamePlayServicePort);
        _distributedLockFactory = distributedLockFactory;
        _serviceProvider = serviceProvider;
        _rabbitMQClient = rabbitMQClient;
        _backgroundJobClient = backgroundJobClient;
        _redisCache = redisCache;

        _gamesService = gamesService;
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
                await HandleConnectionMessage(message, clientEndpoint);
                break;

            case MessageType.Command:
                // Обработка команды
                break;

            case MessageType.Surrender:
                // Обработка сдачи
                break;

            default:
                _logger.LogWarning($"Неизвестный тип сообщения: {message.MessageType}");
                break;
        }

        var (found, gameSession) = await _redisCache.TryGetAsync<GameSession>($"{GameSessionKeyPrefix}:{message.GameId}");
        if (found && gameSession!.GameState != GameState.WaitingForPlayers)
        {
            await SendAllPlayersCurrentRoundState(gameSession);
        }
    }

    private async Task HandleConnectionMessage(IncomingMessage message, IPEndPoint clientEndpoint)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var connectionBL = scope.ServiceProvider.GetRequiredService<IConnectionsBL>();

            if (JsonHelper.TryDeserialize<ConnectionMessage>(message.Message, out var connectionMessage) && connectionMessage != null)
            {
                var session = await connectionBL.GetUserSession(message.AuthToken, connectionMessage.SessionId);
                if (session != null)
                {
                    var lockKey = $"lock:game:{session.GameId}";
                    int maxRetryAttempts = 3;
                    int attempt = 0;
                    bool lockAcquired = false;

                    while (attempt < maxRetryAttempts && !lockAcquired)
                    {
                        attempt++;
                        await using (var redLock = await _distributedLockFactory.CreateLockAsync(
                                resource: lockKey,
                                expiryTime: TimeSpan.FromSeconds(30),
                                waitTime: TimeSpan.FromSeconds(10),
                                retryTime: TimeSpan.FromMilliseconds(200)))
                        {
                            if (redLock.IsAcquired)
                            {
                                lockAcquired = true;
                                var (found, gameSession) = await _redisCache.TryGetAsync<GameSession>($"{GameSessionKeyPrefix}:{session.GameId}");
                                if (found)
                                {
                                    await connectionBL.HandleConnection(gameSession!, session, clientEndpoint);
                                    connectionBL.UpdateGameState(gameSession!);
                                }
                                else
                                {
                                    gameSession = await connectionBL.CreateGameSession(session, clientEndpoint);
                                    _backgroundJobClient.Schedule(() => CloseGameSession(session.GameId), TimeSpan.FromMinutes(2));

                                }

                                await _redisCache.SetAsync($"{GameSessionKeyPrefix}:{message.GameId}", gameSession);

                                return;
                            }
                            else
                            {
                                _logger.LogWarning($"Попытка {attempt}: не удалось получить блокировку для игры {session.GameId}");
                            }
                        }

                        if (!lockAcquired)
                        {
                            await Task.Delay(500);
                        }
                    }
                    if (!lockAcquired)
                    {
                        _logger.LogError($"Не удалось получить блокировку для игры {session.GameId} после {maxRetryAttempts} попыток.");
                    }
                }
                else
                {
                    _logger.LogError($"Ошибка при нахождении игроков сессии: {session} по {connectionMessage.SessionId}");
                }
            }
            else
            {
                _logger.LogError($"Ошибка десериализации JSON: {message}");
            }
        }
    }

    private async Task SendAllPlayersCurrentRoundState(GameSession gameSession)
    {
        foreach (var player in gameSession.Players)
        {
            foreach (var clientEndpoint in player.IPEndPoints)
            {
                string json = JsonSerializer.Serialize(gameSession.RoundState);
                byte[] responseData = Encoding.UTF8.GetBytes(json);
                await _udpServer.SendAsync(responseData, responseData.Length, clientEndpoint);
            }
        }
    }

    private async Task CloseGameSession(string gameId)
    {
        var (found, gameSession) = await _redisCache.TryGetAsync<GameSession>($"{GameSessionKeyPrefix}:{gameId}");
        if (found && gameSession!.GameState == GameState.WaitingForPlayers)
        {
            await _gamesService.CloseAsync(new CloseGameRequest() { Id = gameId });
            await _redisCache.RemoveAsync($"{GameSessionKeyPrefix}:{gameId}");
        }
    }
}