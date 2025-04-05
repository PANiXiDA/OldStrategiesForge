using GamePlayService.Infrastructure.Enums;
using GamePlayService.Infrastructure.Models;
using GamePlayService.Infrastructure.Responses;
using System.Net;

namespace GamePlayService.Messaging.Interfaces;

public interface IMessageSender
{
    Task SendConnectionConfirmed(IPEndPoint clientEndpoint, Guid? messageId);
    Task SendDeploymentStart(GameSession gameSession);
    Task SendCurrentRoundState(GameSession gameSession);
    Task SendGameStart(GameSession gameSession);
    Task SendCommandDone(GameSession gameSession, IPEndPoint ackMessageClientEndpoint, Guid ackMessageId);
    Task SendGameEnd(GameSession gameSession, List<GameResult> gameResults);
    Task SendGameClosed(GameSession gameSession);
    Task SendClientMessageAck(IPEndPoint clientEndpoint, Guid messageId);
    Task SendRepeatMessage(IPEndPoint clientEndpoint, OutgoingMessageType messageType, Guid messageId, string repeatMessage);
}

