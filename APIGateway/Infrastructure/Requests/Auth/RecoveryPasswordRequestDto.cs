using Profile.Auth.Gen;
using System.ComponentModel.DataAnnotations;

namespace APIGateway.Infrastructure.Requests.Auth;

public class RecoveryPasswordRequestDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public RecoveryPasswordRequest RecoveryPasswordRequestDtoToProto()
    {
        return new RecoveryPasswordRequest { Email = Email };

    }
}
