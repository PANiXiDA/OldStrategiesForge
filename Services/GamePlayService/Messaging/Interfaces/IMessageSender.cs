using GamePlayService.Infrastructure.Enums;
using GamePlayService.Infrastructure.Models;
using GamePlayService.Infrastructure.Responses;
using System.Net;

namespace GamePlayService.Messaging.Interfaces;

public interface IMessageSender
{
    Task SendClientMessageAckAsync(IPEndPoint clientEndpoint, Guid messageId);
    Task SendRepeatMessageAsync(IPEndPoint clientEndpoint, OutgoingMessageType messageType, Guid messageId, string repeatMessage);
    Task SendConnectionConfirmedAsync(IPEndPoint clientEndpoint, Guid? messageId);
    Task SendDeploymentStartAsync(GameSession gameSession);
    Task SendCurrentRoundStateAsync(GameSession gameSession);
    Task SendGameStartAsync(GameSession gameSession);
    Task SendCommandDoneAsync(GameSession gameSession, IPEndPoint ackMessageClientEndpoint, Guid ackMessageId);
    Task SendGameEndAsync(GameSession gameSession, List<GameResult> gameResults);
    Task SendGameClosedAsync(GameSession gameSession);
}

