using Profile.Auth.Gen;
using System.ComponentModel.DataAnnotations;

namespace APIGateway.Infrastructure.Requests.Auth;

public class RefreshTokenRequestDto
{
    [Required(ErrorMessage = "Refresh token is required.")]
    public string RefreshToken { get; set; } = string.Empty;

    public RefreshTokenRequest RefreshTokenRequestDtoToProto()
    {
        return new RefreshTokenRequest
        {
            RefreshToken = RefreshToken
        };
    }
}
