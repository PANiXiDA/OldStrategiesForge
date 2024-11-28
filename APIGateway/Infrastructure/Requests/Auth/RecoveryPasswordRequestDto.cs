using Profile.Auth.Gen;
using System.ComponentModel.DataAnnotations;

namespace APIGateway.Infrastructure.Requests.Auth;

public class RecoveryPasswordRequestDto
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; } = string.Empty;

    public RecoveryPasswordRequest RecoveryPasswordRequestDtoToProto()
    {
        return new RecoveryPasswordRequest { Email = Email };

    }
}
