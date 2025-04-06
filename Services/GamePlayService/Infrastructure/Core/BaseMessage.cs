namespace GamePlayService.Infrastructure.Core;

public class BaseMessage
{
    public Guid MessageId { get; set; }
    public bool NeedAck { get; set; }
    public Guid? AckMessageId { get; set; }

    public BaseMessage(
        Guid messageId,
        bool needAck,
        Guid? ackMessageId)
    {
        MessageId = messageId;
        NeedAck = needAck;
        AckMessageId = ackMessageId;
    }
}
