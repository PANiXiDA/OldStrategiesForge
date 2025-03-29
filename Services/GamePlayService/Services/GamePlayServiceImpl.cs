using Common.Constants;
using GameEngine.Domains;
using GamePlayService.BL.BL.Interfaces;
using GamePlayService.Extensions.Helpers;
using GamePlayService.Infrastructure.Enums;
using GamePlayService.Infrastructure.Models;
using GamePlayService.Infrastructure.Requests;
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

    private readonly IConnectionsBL _connectionBL;

    private static readonly ConcurrentDictionary<string, GameSession> _games = new(); // ключ - id игры

    public GamePlayServiceImpl(
        ILogger<GamePlayServiceImpl> logger,
        IConnectionsBL connectionBL)
    {
        _logger = logger;

        _udpServer = new UdpClient(PortsConstants.GamePlayServicePort);

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
                if (JsonHelper.TryDeserialize<ConnectionMessage>(message.Message, out var connectionMessage) && connectionMessage != null)
                {
                    var session = await _connectionBL.GetUserSession(message.AuthToken, connectionMessage.SessionId);
                    if (session != null)
                    {
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
                break;

            case MessageType.Command:
                break;

            case MessageType.Surrender:
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