using Profile.Auth.Gen;

namespace APIGateway.Infrastructure.Responses.Auth;

public class RefreshTokenResponseDto
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;

    public static RefreshTokenResponseDto RefreshTokenResponseDtoFromProto(RefreshTokenResponse loginPlayerResponse)
    {
        return new RefreshTokenResponseDto()
        {
            AccessToken = loginPlayerResponse.AccessToken,
            RefreshToken = loginPlayerResponse.RefreshToken
        };
    }
}
