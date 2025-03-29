using Common.Constants;
using GamePlayService.BL.BL.Interfaces;
using GamePlayService.Extensions.Helpers;
using GamePlayService.Infrastructure.Enums;
using GamePlayService.Infrastructure.Models;
using GamePlayService.Infrastructure.Requests;
using RedLockNet;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace GamePlayService.Services;

public class GamePlayServiceImpl : BackgroundService
{
    private ILogger<GamePlayServiceImpl> _logger;

    private readonly UdpClient _udpServer;
    private readonly IDistributedLockFactory _distributedLockFactory;

    private readonly IConnectionsBL _connectionBL;

    private static readonly ConcurrentDictionary<string, GameSession> _games = new(); // ключ - id игры

    public GamePlayServiceImpl(
        ILogger<GamePlayServiceImpl> logger,
        IDistributedLockFactory distributedLockFactory,
        IConnectionsBL connectionBL)
    {
        _logger = logger;

        _udpServer = new UdpClient(PortsConstants.GamePlayServicePort);
        _distributedLockFactory = distributedLockFactory;

        _connectionBL = connectionBL;
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

        if (_games.TryGetValue(message.GameId, out var gameSession) && gameSession.GameState != GameState.GameInitialization)
        {
            await SendAllPlayersCurrentRoundState(gameSession);
        }
    }

    private async Task HandleConnectionMessage(IncomingMessage message, IPEndPoint clientEndpoint)
    {
        if (JsonHelper.TryDeserialize<ConnectionMessage>(message.Message, out var connectionMessage) && connectionMessage != null)
        {
            var session = await _connectionBL.GetUserSession(message.AuthToken, connectionMessage.SessionId);
            if (session != null)
            {
                var lockKey = $"lock:game:{session.GameId}";
                int maxRetryAttempts = 3; // Максимальное число попыток
                int attempt = 0;
                bool lockAcquired = false;

                while (attempt < maxRetryAttempts && !lockAcquired)
                {
                    attempt++;
                    await using (var redLock = await _distributedLockFactory.CreateLockAsync(
                            resource: lockKey,
                            expiryTime: TimeSpan.FromSeconds(30),
                            waitTime: TimeSpan.FromSeconds(10),
                            retryTime: TimeSpan.FromMilliseconds(200) 
                        ))
                    {
                        if (redLock.IsAcquired)
                        {
                            lockAcquired = true;
                            if (_games.TryGetValue(session.GameId, out var connectionGameSession))
                            {
                                await _connectionBL.HandleConnection(connectionGameSession, session, clientEndpoint);
                                _connectionBL.UpdateGameState(connectionGameSession);
                            }
                            else
                            {
                                connectionGameSession = await _connectionBL.CreateGameSession(session, clientEndpoint);
                                _games[session.GameId] = connectionGameSession;
                            }

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
}