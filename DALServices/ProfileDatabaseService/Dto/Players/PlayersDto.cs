using Auth.Database.Gen;

namespace ProfileDatabaseService.Dto.Players;

public class PlayersDto
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

    public PlayersDto PlayersDtoFromProto(RegistrationPlayerRequest registrationPlayerRequest)
    {
        return new PlayersDto()
        {
            Id = registrationPlayerRequest.Id,
            CreatedAt = registrationPlayerRequest.CreatedAt.ToDateTime(),
            UpdatedAt = registrationPlayerRequest.UpdatedAt.ToDateTime(),
            DeletedAt = registrationPlayerRequest.DeletedAt?.ToDateTime(),
            Email = registrationPlayerRequest.Email,
            Password = registrationPlayerRequest.Password,
            Nickname = registrationPlayerRequest.Nickname,
            Confirmed = registrationPlayerRequest.Confirmed,
            Blocked = registrationPlayerRequest.Blocked,
            Role = registrationPlayerRequest.Role,
            LastLogin = registrationPlayerRequest.LastLogin.ToDateTime(),
            AvatarId = registrationPlayerRequest.AvatarId,
            FrameId = registrationPlayerRequest.FrameId,
            Games = registrationPlayerRequest.Games,
            Wins = registrationPlayerRequest.Wins,
            Loses = registrationPlayerRequest.Loses,
            Mmr = registrationPlayerRequest.Mmr,
            Rank = registrationPlayerRequest.Rank,
            Premium = registrationPlayerRequest.Premium,
            Gold = registrationPlayerRequest.Gold,
            Level = registrationPlayerRequest.Level,
            Experience = registrationPlayerRequest.Experience
        };
    }
}
