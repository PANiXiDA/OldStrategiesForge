using System.ComponentModel.DataAnnotations;

namespace ProfileApiService.Infrastructure.Models.Players;

public class CreatePlayerDto
{
    [Required]
    public string Nickname { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    [Required]
    public string RepeatPassword { get; set; } = string.Empty;
}
