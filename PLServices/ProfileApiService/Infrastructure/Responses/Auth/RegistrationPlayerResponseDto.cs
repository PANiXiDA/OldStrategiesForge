using Auth.Backend.Gen;
namespace ProfileApiService.Infrastructure.Responses.Auth;

public class RegistrationPlayerResponseDto
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Nickname { get; set; } = string.Empty;
    public bool Confirmed { get; set; }
    public bool Blocked { get; set; }
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

    public RegistrationPlayerResponseDto RegistrationPlayerResponseDtoFromProto(RegistrationPlayerResponse createPlayerResponse)
    {
        return new RegistrationPlayerResponseDto()
        {
            Id = createPlayerResponse.Id,
            CreatedAt = createPlayerResponse.CreatedAt.ToDateTime().ToLocalTime(),
            UpdatedAt = createPlayerResponse.UpdatedAt.ToDateTime().ToLocalTime(),
            DeletedAt = createPlayerResponse.DeletedAt?.ToDateTime().ToLocalTime(),
            Email = createPlayerResponse.Email,
            Password = createPlayerResponse.Password,
            Nickname = createPlayerResponse.Nickname,
            Confirmed = createPlayerResponse.Confirmed,
            Blocked = createPlayerResponse.Blocked,
            Role = createPlayerResponse.Role,
            LastLogin = createPlayerResponse.LastLogin.ToDateTime().ToLocalTime(),
            AvatarId = createPlayerResponse.AvatarId,
            FrameId = createPlayerResponse.FrameId,
            Games = createPlayerResponse.Games,
            Wins = createPlayerResponse.Wins,
            Loses = createPlayerResponse.Loses,
            Mmr = createPlayerResponse.Mmr,
            Rank = createPlayerResponse.Rank,
            Premium = createPlayerResponse.Premium,
            Gold = createPlayerResponse.Gold,
            Level = createPlayerResponse.Level,
            Experience = createPlayerResponse.Experience
        };
    }
}
