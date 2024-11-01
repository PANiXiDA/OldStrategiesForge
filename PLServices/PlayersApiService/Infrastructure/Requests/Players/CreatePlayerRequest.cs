using System.ComponentModel.DataAnnotations;

namespace PlayersApiService.Infrastructure.Requests.Players;

public class CreatePlayerRequest
{
    [Required]
    public string Nickname { get; set; } = string.Empty;

    [Required]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    [Required]
    public string RepeatPassword { get; set; } = string.Empty;
}
