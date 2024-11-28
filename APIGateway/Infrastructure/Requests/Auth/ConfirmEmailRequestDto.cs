using Profile.Auth.Gen;
using System.ComponentModel.DataAnnotations;

namespace APIGateway.Infrastructure.Requests.Auth;

public class ConfirmEmailRequestDto
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; } = string.Empty;

    public ConfirmEmailRequest ConfirmEmailRequestDtoToProto()
    {
        return new ConfirmEmailRequest { Email = Email };
    }
}
