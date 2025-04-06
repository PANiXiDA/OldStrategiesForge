using Common.Helpers;
using GamePlayService.Common;
using GamePlayService.Handlers.Interfaces;
using GamePlayService.Infrastructure.Enums;
using GamePlayService.Infrastructure.Requests;
using GamePlayService.Messaging.Interfaces;
using Hangfire;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Tools.Redis;

namespace GamePlayService.Services;

public class GamePlayServiceImpl : BackgroundService
{
    private ILogger<GamePlayServiceImpl> _logger;

    private readonly UdpClient _udpServer;

    private readonly IServiceProvider _serviceProvider;

    private readonly IBackgroundJobClient _backgroundJobClient;
    private readonly IRedisCache _redisCache;

    public GamePlayServiceImpl(
        ILogger<GamePlayServiceImpl> logger,
        UdpClient udpClient,
        IServiceProvider serviceProvider,
        IBackgroundJobClient backgroundJobClient,
        IRedisCache redisCache)
    {
        _logger = logger;

        _udpServer = udpClient;
        _serviceProvider = serviceProvider;

        _backgroundJobClient = backgroundJobClient;
        _redisCache = redisCache;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var received = await _udpServer.ReceiveAsync(stoppingToken);
                var clientEndpoint = received.RemoteEndPoint;
                var incomingMessage = Encoding.UTF8.GetString(received.Buffer);

                await HandleIncomingMessage(incomingMessage, clientEndpoint);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обработке входящего сообщения");
            }
        }
    }

    private async Task HandleIncomingMessage(string incomingMessage, IPEndPoint clientEndpoint)
    {
        using var scope = _serviceProvider.CreateScope();

        if (!JsonHelper.TryDeserialize<IncomingMessage>(incomingMessage, out var message) || message == null)
        {
            _logger.LogError($"Ошибка десериализации JSON: {incomingMessage}");
            return;
        }

        var (found, processedMessage) = await _redisCache.TryGetAsync<IncomingMessage>($"{Constants.ProcessedMessageKeyPrefix}:{message.MessageId}");
        if (found)
        {
            await scope.ServiceProvider.GetRequiredService<IMessageSender>().SendClientMessageAckAsync(clientEndpoint, message.MessageId);
            return;
        }

        if (message.NeedAck)
        {
            await _redisCache.SetAsync($"{Constants.ClientMessageAckKeyPrefix}:{message.MessageId}", clientEndpoint, TimeSpan.FromMinutes(1));
            _backgroundJobClient.Schedule(() => scope.ServiceProvider.GetRequiredService<IMessageTasks>().CheckClientMessageAck(message.MessageId), TimeSpan.FromSeconds(3));
        }

        if (message.AckMessageId.HasValue)
        {
            await _redisCache.RemoveAsync($"{Constants.ServerMessageAckKeyPrefix}:{message.AckMessageId.Value}");
        }

        await _redisCache.SetAsync($"{Constants.ProcessedMessageKeyPrefix}:{message.MessageId}", message);

        await ChooseHandleStrategy(message, clientEndpoint);
    }

    private async Task ChooseHandleStrategy(IncomingMessage message, IPEndPoint clientEndpoint)
    {
        using var scope = _serviceProvider.CreateScope();

        Task handlingTask = message.MessageType switch
        {
            IncomingMessageType.Connection =>
                scope.ServiceProvider.GetRequiredService<IConnectionHandler>().Handle(message, clientEndpoint),
            IncomingMessageType.Deployment =>
                scope.ServiceProvider.GetRequiredService<IDeploymentHandler>().Handle(message, clientEndpoint),
            IncomingMessageType.Command =>
                scope.ServiceProvider.GetRequiredService<ICommandHandler>().Handle(message, clientEndpoint),
            IncomingMessageType.Surrender =>
                scope.ServiceProvider.GetRequiredService<ISurrenderHandler>().Handle(message, clientEndpoint),
            IncomingMessageType.MessageAck => Task.CompletedTask,
            _ => LogUnknownMessageAsync(message.MessageType)
        };

        await handlingTask;
    }

    private Task LogUnknownMessageAsync(IncomingMessageType messageType)
    {
        _logger.LogWarning($"Неизвестный тип сообщения: {messageType}");
        return Task.CompletedTask;
    }
}