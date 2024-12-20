using System.ComponentModel.DataAnnotations;

namespace APIGateway.Infrastructure.Requests.Chats;

public class ChatMessageRequestDto
{
    [Required(ErrorMessage = "Content is required.")]
    public string Content { get; set; } = string.Empty;
}
