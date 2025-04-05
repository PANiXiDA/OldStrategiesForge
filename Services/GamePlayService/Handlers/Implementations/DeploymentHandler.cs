using Common.Helpers;
using GamePlayService.BL.BL.Interfaces;
using GamePlayService.Common;
using GamePlayService.Handlers.Interfaces;
using GamePlayService.Infrastructure.Enums;
using GamePlayService.Infrastructure.Models;
using GamePlayService.Infrastructure.Requests;
using GamePlayService.Messaging.Interfaces;
using System.Net;
using Tools.Redis;

namespace GamePlayService.Handlers.Implementations;

public class DeploymentHandler : IDeploymentHandler
{
    private readonly ILogger<DeploymentHandler> _logger;

    private readonly IRedisCache _redisCache;

    private readonly IMessageSender _messageSender;

    private readonly IDeploymentsBL _deploymentsBL;

    public DeploymentHandler(
        ILogger<DeploymentHandler> logger,
        IRedisCache redisCache,
        IMessageSender messageSender,
        IDeploymentsBL deploymentsBL)
    {
        _logger = logger;

        _deploymentsBL = deploymentsBL;

        _messageSender = messageSender;

        _redisCache = redisCache;
    }

    public async Task Handle(IncomingMessage message, IPEndPoint clientEndpoint)
    {
        if (!JsonHelper.TryDeserialize<DeploymentMessage>(message.Message, out var deploymentMessage) || deploymentMessage == null)
        {
            _logger.LogError("Ошибка десериализации JSON: {Message}", message.Message);
            return;
        }

        var (found, gameSession) = await _redisCache.TryGetAsync<GameSession>($"{Constants.GameSessionKeyPrefix}:{message.GameId}");
        if (!found || gameSession!.GameState != GameState.Deployment)
        {
            _logger.LogError($"Игра по id: {message.GameId} не найдена в пуле текущих игр или не находится в статусе Deployment: {gameSession}");
            return;
        }

        if (!_deploymentsBL.ValidateDeployment(gameSession, message.PlayerId, deploymentMessage.Deployment))
        {
            _logger.LogError($"Ошибка при валидации следующей расстановки: {deploymentMessage.Deployment}");
            return;
        }

        _deploymentsBL.ApplyDeployment(gameSession.RoundState.Grid, deploymentMessage.Deployment); // TODO: добавить подтверждение игроком расстановки

        if (gameSession.Players.All(player => player.ConfirmedDeployment))
        {
            await _messageSender.SendGameStart(gameSession);
        }

        await _messageSender.SendClientMessageAck(clientEndpoint, message.MessageId);

        await _redisCache.SetAsync($"{Constants.GameSessionKeyPrefix}:{message.GameId}", gameSession);
    }
}
