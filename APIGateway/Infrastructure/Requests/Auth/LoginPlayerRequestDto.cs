using Profile.Auth.Gen;
using System.ComponentModel.DataAnnotations;

namespace APIGateway.Infrastructure.Requests.Auth;

public class LoginPlayerRequestDto
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
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

