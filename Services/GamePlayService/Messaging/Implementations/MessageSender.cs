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

namespace GamePlayService.Messaging.Implementations;

public class MessageSender : IMessageSender
{
    private readonly UdpClient _udpServer;

    private readonly IRedisCache _redisCache;
    private readonly IBackgroundJobClient _backgroundJobClient;

    private readonly Lazy<IMessageTasks> _messageTasks;

    public MessageSender(
        UdpClient udpServer,
        IRedisCache redisCache,
        IBackgroundJobClient backgroundJobClient,
        Lazy<IMessageTasks> messageTasks)
    {
        _udpServer = udpServer;

        _redisCache = redisCache;
        _backgroundJobClient = backgroundJobClient;

        _messageTasks = messageTasks;
    }

    public async Task SendClientMessageAckAsync(IPEndPoint clientEndpoint, Guid ackMessageId)
    {
        await SendMessageAsync(
            clientEndpoints: new List<IPEndPoint>() { clientEndpoint },
            messageType: OutgoingMessageType.MessageAck,
            payload: string.Empty,
            needAck: false,
            ackMessageId: ackMessageId);
    }

    public async Task SendRepeatMessageAsync(IPEndPoint clientEndpoint, OutgoingMessageType messageType, Guid messageId, string repeatMessage)
    {
        await SendMessageAsync(
            clientEndpoints: new List<IPEndPoint>() { clientEndpoint },
            messageType: messageType,
            payload: repeatMessage,
            needAck: true,
            ackMessageId: null,
            messageId: messageId);
    }

    public async Task SendConnectionConfirmedAsync(IPEndPoint clientEndpoint, Guid? ackMessageId)
    {
        await SendMessageAsync(
            clientEndpoints: new List<IPEndPoint>() { clientEndpoint },
            messageType: OutgoingMessageType.ConnectionConfirmed,
            payload: string.Empty,
            needAck: false,
            ackMessageId: ackMessageId);

        await _redisCache.RemoveAsync($"{Constants.ClientMessageAckKeyPrefix}:{ackMessageId}");
    }

    public async Task SendDeploymentStartAsync(GameSession gameSession)
    {
        foreach (var player in gameSession.Players)
        {
            await SendMessageAsync(
                clientEndpoints: player.IPEndPoints,
                messageType: OutgoingMessageType.DeploymentStart,
                payload: new DeploymentStartMessage(
                    grid: gameSession.RoundState.Grid,
                    units: player.Units),
                needAck: true,
                ackMessageId: null);
        }
    }

    public async Task SendCurrentRoundStateAsync(GameSession gameSession)
    {
        foreach (var player in gameSession.Players)
        {
            await SendMessageAsync(
                clientEndpoints: player.IPEndPoints,
                messageType: OutgoingMessageType.Command,
                payload: gameSession.RoundState,
                needAck: true,
                ackMessageId: null);
        }
    }

    public async Task SendGameStartAsync(GameSession gameSession)
    {
        foreach (var player in gameSession.Players)
        {
            await SendMessageAsync(
                clientEndpoints: player.IPEndPoints,
                messageType: OutgoingMessageType.GameStart,
                payload: gameSession.RoundState,
                needAck: true,
                ackMessageId: null);
        }
    }

    public async Task SendCommandDoneAsync(GameSession gameSession, IPEndPoint ackMessageClientEndpoint, Guid ackMessageId)
    {
        foreach (var player in gameSession.Players)
        {
            await SendMessageAsync(
                clientEndpoints: player.IPEndPoints,
                messageType: OutgoingMessageType.Command,
                payload: gameSession.RoundState,
                needAck: true,
                ackMessageId: player.IPEndPoints.Contains(ackMessageClientEndpoint) ? ackMessageId : null);
        }
    }

    public async Task SendGameEndAsync(GameSession gameSession, List<GameResult> gameResults)
    {
        foreach (var player in gameSession.Players)
        {
            await SendMessageAsync(
                clientEndpoints: player.IPEndPoints,
                messageType: OutgoingMessageType.GameEnd,
                payload: new GameEndMessage(gameResults: gameResults),
                needAck: true,
                ackMessageId: null);
        }
    }

    public async Task SendGameClosedAsync(GameSession gameSession)
    {
        foreach (var player in gameSession.Players)
        {
            await SendMessageAsync(
                clientEndpoints: player.IPEndPoints,
                messageType: OutgoingMessageType.GameClosed,
                payload: string.Empty,
                needAck: true,
                ackMessageId: null);
        }
    }

    private async Task SendMessageAsync<T>(
        IEnumerable<IPEndPoint> clientEndpoints,
        OutgoingMessageType messageType,
        T payload,
        bool needAck = true,
        Guid? ackMessageId = null,
        Guid? messageId = null)
    {
        var message = new OutgoingMessage<T>(
            messageId: messageId ?? Guid.NewGuid(),
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
                _backgroundJobClient.Schedule(() => _messageTasks.Value.CheckServerMessageAck(message.MessageId, messageType, endpoint, waitingSeconds),
                    TimeSpan.FromSeconds(waitingSeconds));
            }
        }
    }
}
