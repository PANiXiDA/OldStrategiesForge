using Profile.Auth.Gen;

namespace APIGateway.Infrastructure.Requests.Auth;

public class RefreshTokenRequestDto
{
    public string RefreshToken { get; set; } = string.Empty;

    public RefreshTokenRequest RefreshTokenRequestDtoToProto()
    {
        return new RefreshTokenRequest
        {
            RefreshToken = RefreshToken
        };
    }
}
