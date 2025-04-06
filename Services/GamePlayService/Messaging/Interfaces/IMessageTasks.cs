using GamePlayService.Infrastructure.Enums;
using System.Net;

namespace GamePlayService.Messaging.Interfaces;

public interface IMessageTasks
{
    Task CheckClientMessageAck(Guid messageId);
    Task CheckServerMessageAck(Guid messageId, OutgoingMessageType messageType, IPEndPoint clientEndpoint, int waitingSeconds);
    Task CloseGameSession(string gameId);
    Task EndDeployment(string gameId);
    Task EndTurn(string gameId, Guid gameObjectId);
    Task GameEnd(string gameId, int loserId);
}
