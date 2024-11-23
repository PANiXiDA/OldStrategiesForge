using Profile.Auth.Gen;
using System.ComponentModel.DataAnnotations;

namespace APIGateway.Infrastructure.Requests.Auth;

public class LogoutRequestDto
{
    [Required]
    public string RefreshToken { get; set; } = string.Empty;

    public LogoutRequest LogoutRequestDtoToProto()
    {
        return new LogoutRequest { RefreshToken = RefreshToken };
    }
}
