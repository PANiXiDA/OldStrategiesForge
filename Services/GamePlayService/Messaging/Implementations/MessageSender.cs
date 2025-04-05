using GamePlayService.Infrastructure.Enums;
using GamePlayService.Infrastructure.Responses.Core;
using Hangfire;
using System.Net.Sockets;
using System.Net;
using System.Text;
using Tools.Redis;
using System.Text.Json;
using GamePlayService.Infrastructure.Models;
using GamePlayService.Infrastructure.Responses;
using GamePlayService.Common;
using GamePlayService.Messaging.Interfaces;
using Microsoft.AspNetCore.SignalR.Protocol;

namespace GamePlayService.Messaging.Implementations;

public class MessageSender : IMessageSender
{
    private readonly UdpClient _udpServer;
    private readonly IRedisCache _redisCache;
    private readonly IBackgroundJobClient _backgroundJobClient;

    public MessageSender(UdpClient udpServer, IRedisCache redisCache, IBackgroundJobClient backgroundJobClient)
    {
        _udpServer = udpServer;
        _redisCache = redisCache;
        _backgroundJobClient = backgroundJobClient;
    }

    public async Task SendConnectionConfirmed(IPEndPoint clientEndpoint, Guid? ackMessageId)
    {
        await SendMessageAsync(
            clientEndpoints: new List<IPEndPoint>() { clientEndpoint },
            messageType: OutgoingMessageType.ConnectionConfirmed,
            payload: string.Empty,
            needAck: false,
            ackMessageId: ackMessageId);

        await _redisCache.RemoveAsync($"{Constants.ClientMessageAckKeyPrefix}:{ackMessageId}");
    }

    public async Task SendDeploymentStart(GameSession gameSession)
    {
        foreach (var player in gameSession.Players)
        {
            await SendMessageAsync(
                clientEndpoints: player.IPEndPoints,
                messageType: OutgoingMessageType.DeploymentStart,
                payload: new DeploymentStartMessage(
                    grid: gameSession.RoundState.Grid,
                    units: player.Units));
        }
    }

    public async Task SendCurrentRoundState(GameSession gameSession)
    {
        foreach (var player in gameSession.Players)
        {
            foreach (var clientEndpoint in player.IPEndPoints)
            {
                var message = new OutgoingMessage<RoundState>(
                    messageId: Guid.NewGuid(),
                    needAck: true,
                    ackMessageId: null,
                    messageType: OutgoingMessageType.Command,
                    message: gameSession.RoundState);

                string json = JsonSerializer.Serialize(message);
                byte[] responseData = Encoding.UTF8.GetBytes(json);

                await _udpServer.SendAsync(responseData, responseData.Length, clientEndpoint);

                var jsonMessage = JsonSerializer.Serialize(message);
                await _redisCache.SetAsync($"{Constants.ServerMessageAckKeyPrefix}:{message.MessageId}", jsonMessage, TimeSpan.FromMinutes(1));

                int waitingSeconds = 5;
                _backgroundJobClient.Schedule(() => CheckServerMessageAck(message.MessageId, message.MessageType, clientEndpoint, waitingSeconds),
                    TimeSpan.FromSeconds(waitingSeconds));
            }
        }
    }

    public async Task SendGameStart(GameSession gameSession)
    {
        foreach (var player in gameSession.Players)
        {
            foreach (var clientEndpoint in player.IPEndPoints)
            {
                var message = new OutgoingMessage<RoundState>(
                    messageId: Guid.NewGuid(),
                    needAck: true,
                    ackMessageId: null,
                    messageType: OutgoingMessageType.GameStart,
                    message: gameSession.RoundState);

                string json = JsonSerializer.Serialize(message);
                byte[] responseData = Encoding.UTF8.GetBytes(json);

                await _udpServer.SendAsync(responseData, responseData.Length, clientEndpoint);

                var jsonMessage = JsonSerializer.Serialize(message);
                await _redisCache.SetAsync($"{Constants.ServerMessageAckKeyPrefix}:{message.MessageId}", jsonMessage, TimeSpan.FromMinutes(1));

                int waitingSeconds = 5;
                _backgroundJobClient.Schedule(() => CheckServerMessageAck(message.MessageId, message.MessageType, clientEndpoint, waitingSeconds),
                    TimeSpan.FromSeconds(waitingSeconds));
            }
        }
    }

    public async Task SendCommandDone(GameSession gameSession, IPEndPoint ackMessageClientEndpoint, Guid ackMessageId)
    {
        foreach (var player in gameSession.Players)
        {
            foreach (var clientEndpoint in player.IPEndPoints)
            {
                var message = new OutgoingMessage<RoundState>(
                    messageId: Guid.NewGuid(),
                    needAck: true,
                    ackMessageId: clientEndpoint == ackMessageClientEndpoint ? ackMessageId : null,
                    messageType: OutgoingMessageType.Command,
                    message: gameSession.RoundState);

                string json = JsonSerializer.Serialize(message);
                byte[] responseData = Encoding.UTF8.GetBytes(json);

                await _udpServer.SendAsync(responseData, responseData.Length, clientEndpoint);

                var jsonMessage = JsonSerializer.Serialize(message);
                await _redisCache.SetAsync($"{Constants.ServerMessageAckKeyPrefix}:{message.MessageId}", jsonMessage, TimeSpan.FromMinutes(1));

                int waitingSeconds = 5;
                _backgroundJobClient.Schedule(() => CheckServerMessageAck(message.MessageId, message.MessageType, clientEndpoint, waitingSeconds),
                    TimeSpan.FromSeconds(waitingSeconds));
            }
        }
    }

    public async Task SendGameEnd(GameSession gameSession, List<GameResult> gameResults)
    {
        foreach (var player in gameSession.Players)
        {
            foreach (var clientEndpoint in player.IPEndPoints)
            {
                var message = new OutgoingMessage<GameEndMessage>(
                    messageId: Guid.NewGuid(),
                    needAck: true,
                    ackMessageId: null,
                    messageType: OutgoingMessageType.GameEnd,
                    message: new GameEndMessage(gameResults: gameResults));

                string json = JsonSerializer.Serialize(message);
                byte[] responseData = Encoding.UTF8.GetBytes(json);

                await _udpServer.SendAsync(responseData, responseData.Length, clientEndpoint);

                var jsonMessage = JsonSerializer.Serialize(message);
                await _redisCache.SetAsync($"{Constants.ServerMessageAckKeyPrefix}:{message.MessageId}", jsonMessage, TimeSpan.FromMinutes(1));

                int waitingSeconds = 5;
                _backgroundJobClient.Schedule(() => CheckServerMessageAck(message.MessageId, message.MessageType, clientEndpoint, waitingSeconds),
                    TimeSpan.FromSeconds(waitingSeconds));
            }
        }
    }

    public async Task SendGameClosed(GameSession gameSession)
    {
        var message = new OutgoingMessage<string>(
            messageId: Guid.NewGuid(),
            needAck: true,
            ackMessageId: null,
            messageType: OutgoingMessageType.GameClosed,
            message: string.Empty);

        string json = JsonSerializer.Serialize(message);
        byte[] responseData = Encoding.UTF8.GetBytes(json);

        foreach (var player in gameSession.Players)
        {
            foreach (var clientEndpoint in player.IPEndPoints)
            {
                await _udpServer.SendAsync(responseData, responseData.Length, clientEndpoint);

                var jsonMessage = JsonSerializer.Serialize(message);
                await _redisCache.SetAsync($"{Constants.ServerMessageAckKeyPrefix}:{message.MessageId}", jsonMessage, TimeSpan.FromMinutes(1));

                int waitingSeconds = 5;
                _backgroundJobClient.Schedule(() => CheckServerMessageAck(message.MessageId, message.MessageType, clientEndpoint, waitingSeconds),
                    TimeSpan.FromSeconds(waitingSeconds));
            }
        }
    }

    public async Task SendClientMessageAck(IPEndPoint clientEndpoint, Guid ackMessageId)
    {
        var message = new OutgoingMessage<string>(
            messageId: Guid.NewGuid(),
            needAck: false,
            ackMessageId: ackMessageId,
            messageType: OutgoingMessageType.MessageAck,
            message: string.Empty);

        string json = JsonSerializer.Serialize(message);
        byte[] responseData = Encoding.UTF8.GetBytes(json);
        await _udpServer.SendAsync(responseData, responseData.Length, clientEndpoint);

        await _redisCache.RemoveAsync($"{Constants.ClientMessageAckKeyPrefix}:{ackMessageId}");
    }

    public async Task SendRepeatMessage(IPEndPoint clientEndpoint, OutgoingMessageType messageType, Guid messageId, string repeatMessage)
    {
        var message = new OutgoingMessage<string>(
            messageId: messageId,
            needAck: true,
            ackMessageId: null,
            messageType: messageType,
            message: repeatMessage);

        string json = JsonSerializer.Serialize(message);
        byte[] responseData = Encoding.UTF8.GetBytes(json);
        await _udpServer.SendAsync(responseData, responseData.Length, clientEndpoint);
    }

    // TODO: возможно стоит перенести в MessageTasks, но тогда зависимости между этими сервисами нужно будет прокдиывать через Lazy
    private async Task CheckServerMessageAck(Guid messageId, OutgoingMessageType messageType, IPEndPoint clientEndpoint, int waitingSeconds)
    {
        var (found, message) = await _redisCache.TryGetAsync<string>($"{Constants.ServerMessageAckKeyPrefix}:{messageId}");
        if (found && message != null)
        {
            await SendRepeatMessage(clientEndpoint, messageType, messageId, message);
            int newWaitingSeconds = waitingSeconds + 5;
            _backgroundJobClient.Schedule(() => CheckServerMessageAck(messageId, messageType, clientEndpoint, newWaitingSeconds),
                TimeSpan.FromSeconds(newWaitingSeconds));
        }
    }

    private async Task SendMessageAsync<T>(
        IEnumerable<IPEndPoint> clientEndpoints,
        OutgoingMessageType messageType,
        T payload,
        bool needAck = true,
        Guid? ackMessageId = null)
    {
        var message = new OutgoingMessage<T>(
            messageId: Guid.NewGuid(),
            needAck: needAck,
            ackMessageId: ackMessageId,
            messageType: messageType,
            message: payload);

        string json = JsonSerializer.Serialize(message);
        byte[] responseData = Encoding.UTF8.GetBytes(json);

        foreach (var endpoint in clientEndpoints)
        {
            await _udpServer.SendAsync(responseData, responseData.Length, endpoint);

            if (needAck)
            {
                await _redisCache.SetAsync($"{Constants.ServerMessageAckKeyPrefix}:{message.MessageId}", json, TimeSpan.FromMinutes(1));
                int waitingSeconds = 5;
                _backgroundJobClient.Schedule(() => CheckServerMessageAck(message.MessageId, messageType, endpoint, waitingSeconds),
                    TimeSpan.FromSeconds(waitingSeconds));
            }
        }
    }
}
