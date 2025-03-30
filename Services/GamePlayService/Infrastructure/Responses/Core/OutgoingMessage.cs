using GamePlayService.Infrastructure.Enums;

namespace GamePlayService.Infrastructure.Responses.Core;

public class OutgoingMessage<T>
{
    public OutgoingMessageType MessageType { get; set; }
    public T Message { get; set; }


    public OutgoingMessage(
        OutgoingMessageType messageType,
        T message)
    {
        MessageType = messageType;
        Message = message;
    }
}
