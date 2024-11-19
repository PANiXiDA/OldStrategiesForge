using Profile.Auth.Gen;

namespace APIGateway.Infrastructure.Responses.Auth;

public class LoginPlayerResponseDto
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Nickname { get; set; } = string.Empty;
    public DateTime LastLogin { get; set; }
    public int AvatarId { get; set; }
    public int FrameId { get; set; }
    public int Games { get; set; }
    public int Wins { get; set; }
    public int Loses { get; set; }
    public int Mmr { get; set; }
    public int Rank { get; set; }
    public bool Premium { get; set; }
    public int Gold { get; set; }
    public int Level { get; set; }
    public int Experience { get; set; }

    public LoginPlayerResponseDto LoginPlayerResponseDtoFromProto(LoginPlayerResponse createPlayerResponse)
    {
        return new LoginPlayerResponseDto()
        {
            //Id = createPlayerResponse.Id,
            //Email = createPlayerResponse.Email,
            //Password = createPlayerResponse.Password,
            //Nickname = createPlayerResponse.Nickname,
            //LastLogin = createPlayerResponse.LastLogin.ToDateTime().ToLocalTime(),
            //AvatarId = createPlayerResponse.AvatarId,
            //FrameId = createPlayerResponse.FrameId,
            //Games = createPlayerResponse.Games,
            //Wins = createPlayerResponse.Wins,
            //Loses = createPlayerResponse.Loses,
            //Mmr = createPlayerResponse.Mmr,
            //Rank = createPlayerResponse.Rank,
            //Premium = createPlayerResponse.Premium,
            //Gold = createPlayerResponse.Gold,
            //Level = createPlayerResponse.Level,
            //Experience = createPlayerResponse.Experience
        };
    }
}
