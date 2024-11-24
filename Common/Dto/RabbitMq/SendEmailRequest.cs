namespace Common.Dto.RabbitMq;

public class SendEmailRequest
{
    public string Email { get; set; } = string.Empty;
    public int? Id { get; set; }
    public string? Password { get; set; }
}
