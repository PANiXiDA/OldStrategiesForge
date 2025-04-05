using GamePlayService.Extensions.Helpers;
using GamePlayService.Infrastructure.Core;
using GamePlayService.Infrastructure.Enums;

namespace GamePlayService.Infrastructure.Requests;

public class IncomingMessage : BaseMessage
{
    public IncomingMessageType MessageType { get; }
    public int PlayerId { get; }
    public string GameId { get; set; } = string.Empty; // TODO подумать над безопасной передачей
    public string Message { get; set; } = string.Empty;

    public IncomingMessage(
        Guid messageId,
        bool needAck,
        Guid? ackMessageId,
        IncomingMessageType messageType,
        string authToken,
        string gameId,
        string message) : base(messageId, needAck, ackMessageId)
    {
        MessageType = messageType;
        GameId = gameId;
        Message = message;

        PlayerId = JwtHelper.GetPlayerIdFromToken(authToken);
    }
}
