using Common.Helpers;
using GamePlayService.BL.BL.Interfaces;
using GamePlayService.Common;
using GamePlayService.Handlers.Interfaces;
using GamePlayService.Infrastructure.Enums;
using GamePlayService.Infrastructure.Models;
using GamePlayService.Infrastructure.Requests;
using GamePlayService.Messaging.Interfaces;
using Hangfire;
using RedLockNet;
using System.Net;
using Tools.Redis;

namespace GamePlayService.Handlers.Implementations;

public class ConnectionHandler : IConnectionHandler
{
    private readonly ILogger<ConnectionHandler> _logger;

    private readonly IBackgroundJobClient _backgroundJobClient;

    private readonly IRedisCache _redisCache;
    private readonly IDistributedLockFactory _distributedLockFactory;

    private readonly IMessageTasks _messageTasks;
    private readonly IMessageSender _messageSender;

    private readonly IConnectionsBL _connectionsBL;

    public ConnectionHandler(
        ILogger<ConnectionHandler> logger,
        IBackgroundJobClient backgroundJobClient,
        IRedisCache redisCache,
        IDistributedLockFactory distributedLockFactory,
        IMessageTasks messageTasks,
        IMessageSender messageSender,
        IConnectionsBL connectionsBL)
    {
        _logger = logger;

        _backgroundJobClient = backgroundJobClient;

        _redisCache = redisCache;
        _distributedLockFactory = distributedLockFactory;

        _messageTasks = messageTasks;
        _messageSender = messageSender;

        _connectionsBL = connectionsBL;
    }

    public async Task Handle(IncomingMessage message, IPEndPoint clientEndpoint)
    {
        if (!JsonHelper.TryDeserialize<ConnectionMessage>(message.Message, out var connectionMessage) || connectionMessage == null)
        {
            _logger.LogError("Ошибка десериализации JSON: {Message}", message.Message);
            return;
        }

        var session = await _connectionsBL.GetUserSession(message.PlayerId, connectionMessage.SessionId);
        if (session == null)
        {
            _logger.LogError("Сессия не найдена по {SessionId}", connectionMessage.SessionId);
            return;
        }

        var lockKey = $"{Constants.LockKeyPrefix}:{Constants.GameSessionKeyPrefix}:{session.GameId}";
        const int maxRetryAttempts = 3;
        bool lockAcquired = false;

        for (int attempt = 1; attempt <= maxRetryAttempts; attempt++)
        {
            await using (var redLock = await _distributedLockFactory.CreateLockAsync(
                                    resource: lockKey,
                                    expiryTime: TimeSpan.FromSeconds(30),
                                    waitTime: TimeSpan.FromSeconds(10),
                                    retryTime: TimeSpan.FromMilliseconds(200)))
            {
                if (redLock.IsAcquired)
                {
                    lockAcquired = true;

                    var (found, gameSession) = await _redisCache.TryGetAsync<GameSession>($"{Constants.GameSessionKeyPrefix}:{session.GameId}");
                    if (found)
                    {
                        await _connectionsBL.HandleConnection(gameSession!, session, clientEndpoint);
                        _connectionsBL.UpdateGameState(gameSession!);
                    }
                    else
                    {
                        gameSession = await _connectionsBL.CreateGameSession(session, clientEndpoint);
                        _backgroundJobClient.Schedule(() => _messageTasks.CloseGameSession(message.GameId), TimeSpan.FromMinutes(2));
                    }

                    await _messageSender.SendConnectionConfirmed(clientEndpoint, message.MessageId);

                    if (gameSession!.GameState == GameState.Deployment)
                    {
                        gameSession.GameState = GameState.InProgress;
                        await _messageSender.SendDeploymentStart(gameSession);
                        _backgroundJobClient.Schedule(() => _messageTasks.EndDeployment(message.GameId), TimeSpan.FromMinutes(2));
                    }

                    await _redisCache.SetAsync($"{Constants.GameSessionKeyPrefix}:{message.GameId}", gameSession, TimeSpan.FromDays(1));

                    return;
                }
            }

            _logger.LogWarning("Попытка {Attempt}: не удалось получить блокировку для игры {GameId}", attempt, session.GameId);
            await Task.Delay(500);
        }

        if (!lockAcquired)
        {
            _logger.LogError("Не удалось получить блокировку для игры {GameId} после {MaxRetryAttempts} попыток.", session.GameId, maxRetryAttempts);
        }
    }
}
