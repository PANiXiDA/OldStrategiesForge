using Profile.Auth.Gen;

namespace APIGateway.Infrastructure.Responses.Auth;

public class LoginPlayerResponseDto
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;

    public LoginPlayerResponseDto LoginPlayerResponseDtoFromProto(LoginPlayerResponse loginPlayerResponse)
    {
        return new LoginPlayerResponseDto()
        {
            AccessToken = loginPlayerResponse.AccessToken,
            RefreshToken = loginPlayerResponse.RefreshToken
        };
    }
}
