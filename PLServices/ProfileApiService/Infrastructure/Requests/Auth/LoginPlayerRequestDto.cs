using Auth.Backend.Gen;
using System.ComponentModel.DataAnnotations;

namespace ProfileApiService.Infrastructure.Requests.Auth;

public class LoginPlayerRequestDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    public LoginPlayerRequest LoginPlayerRequestDtoToProto()
    {
        return new LoginPlayerRequest
        {
            Email = Email,
            Password = Password
        };
    }
}
