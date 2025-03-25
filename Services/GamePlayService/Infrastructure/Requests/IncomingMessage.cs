using GamePlayService.Infrastructure.Enums;

namespace GamePlayService.Infrastructure.Requests;

public class IncomingMessage
{
    public MessageType MessageType { get; set; }
    public string Message { get; set; } = string.Empty;
}
