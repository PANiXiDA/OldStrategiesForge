using GamePlayService.Infrastructure.Core;
using GamePlayService.Infrastructure.Enums;

namespace GamePlayService.Infrastructure.Responses.Core;

public class OutgoingMessage<T> : BaseMessage
{
    public OutgoingMessageType MessageType { get; set; }
    public T Message { get; set; }


    public OutgoingMessage(
        Guid messageId,
        bool needAck,
        Guid? ackMessageId,
        OutgoingMessageType messageType,
        T message) : base(messageId, needAck, ackMessageId)
    {
        MessageType = messageType;
        Message = message;
    }
}
