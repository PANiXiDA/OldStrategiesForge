using GamePlayService.Handlers.Interfaces;
using GamePlayService.Infrastructure.Requests;
using GamePlayService.Messaging.Interfaces;
using Hangfire;
using System.Net;
using Tools.Redis;

namespace GamePlayService.Handlers.Implementations;

public class SurrenderHandler : ISurrenderHandler
{
    private readonly ILogger<SurrenderHandler> _logger;

    private readonly IBackgroundJobClient _backgroundJobClient;
    private readonly IRedisCache _redisCache;

    private readonly IMessageSender _messageSender;
    private readonly IMessageTasks _messageTasks;

    public SurrenderHandler(
        ILogger<SurrenderHandler> logger,
        IBackgroundJobClient backgroundJobClient,
        IRedisCache redisCache,
        IMessageSender messageSender,
        IMessageTasks messageTasks)
    {
        _logger = logger;

        _backgroundJobClient = backgroundJobClient;
        _redisCache = redisCache;

        _messageSender = messageSender;
        _messageTasks = messageTasks;
    }

    public async Task Handle(IncomingMessage message, IPEndPoint clientEndpoint)
    {
        _backgroundJobClient.Schedule(() => _messageTasks.GameEnd(message.GameId, message.PlayerId), TimeSpan.FromSeconds(1));
        await _messageSender.SendClientMessageAckAsync(clientEndpoint, message.MessageId);
    }
}
