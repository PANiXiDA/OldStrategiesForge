using GamePlayService.Infrastructure.Enums;

namespace GamePlayService.Infrastructure.Requests;

public class IncomingMessage
{
    public MessageType MessageType { get; set; }
    public string AuthToken { get; set; } = string.Empty;
    public string GameId { get; set; } = string.Empty; // TODO подумать над безопасной передачей
    public string Message { get; set; } = string.Empty;
}
