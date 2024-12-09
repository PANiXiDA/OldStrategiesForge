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
    public AvatarDto Avatar { get; set; } = new AvatarDto();


    public static GetPlayerResponseDto GetPlayerResponseDtoFromProto(GetPlayerResponse getPlayerResponse)
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
            Experience = getPlayerResponse.Experience,
            Avatar = new AvatarDto()
            {
                S3Path = getPlayerResponse.Avatar.S3Path,
                Name = getPlayerResponse.Avatar.Name,
                Description = getPlayerResponse.Avatar.Description,
                FileName = getPlayerResponse.Avatar.FileName
            }
        };
    }
}

public class AvatarDto
{
    public string S3Path { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
}