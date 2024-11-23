namespace ProfileService.Dto.RabbitMq;

public class SendEmailResponse
{
    public bool Success { get; set; }
    public string? Error { get; set; }
}
