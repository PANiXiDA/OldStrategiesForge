using Profile.Players.Gen;

namespace APIGateway.Infrastructure.Responses.Players;

public class GetPlayerResponseDto
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Nickname { get; set; } = string.Empty;
    public int Role { get; set; }
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

    public GetPlayerResponseDto GetPlayerResponseDtoFromProto(GetPlayerResponse getPlayerResponse)
    {
        return new GetPlayerResponseDto()
        {
            Id = getPlayerResponse.Id,
            Email = getPlayerResponse.Email,
            Password = getPlayerResponse.Password,
            Nickname = getPlayerResponse.Nickname,
            Role = getPlayerResponse.Role,
            LastLogin = getPlayerResponse.LastLogin.ToDateTime(),
            AvatarId = getPlayerResponse.AvatarId,
            FrameId = getPlayerResponse.FrameId,
            Games = getPlayerResponse.Games,
            Wins = getPlayerResponse.Wins,
            Loses = getPlayerResponse.Loses,
            Mmr = getPlayerResponse.Mmr,
            Rank = getPlayerResponse.Rank,
            Premium = getPlayerResponse.Premium,
            Gold = getPlayerResponse.Gold,
            Level = getPlayerResponse.Level,
            Experience = getPlayerResponse.Experience
        };
    }
}
