using System.ComponentModel.DataAnnotations;

namespace APIGateway.Infrastructure.Requests.Players;

public class UpdateAvatarRequestDto
{
    [Required]
    public int AvatarId { get; set; }
}
