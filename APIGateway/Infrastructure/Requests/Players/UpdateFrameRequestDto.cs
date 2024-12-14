using System.ComponentModel.DataAnnotations;

namespace APIGateway.Infrastructure.Requests.Players;

public class UpdateFrameRequestDto
{
    [Required]
    public int FrameId { get; set; }
}
