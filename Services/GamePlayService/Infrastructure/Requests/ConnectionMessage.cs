namespace GamePlayService.Infrastructure.Requests;

public class ConnectionMessage
{
    public string AuthToken { get; set; } = string.Empty;
    public string SessionId { get; set; } = string.Empty;
    public string BuildId { get; set; } = string.Empty;
}
