using Common.Enums;
using Common;
using Profile.Auth.Gen;
using Profile.Players.Gen;
using Google.Protobuf.WellKnownTypes;

namespace ProfileService.Dto;

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

    public PlayersDto PlayersDtoFromProtoAuth(RegistrationPlayerRequest registrationPlayer)
    {
        return new PlayersDto()
        {
            Id = 0,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            DeletedAt = null,
            Email = registrationPlayer.Email,
            Password = Helpers.GetPasswordHash(registrationPlayer.Password),
            Nickname = registrationPlayer.Nickname,
            Confirmed = true, //как добавлю подтверждение аккаунта, нужно поменять
            Blocked = false,
            Role = (int)PlayerRole.Player,
            LastLogin = DateTime.UtcNow,
            AvatarId = 1,
            FrameId = 1,
            Games = 0,
            Wins = 0,
            Loses = 0,
            Mmr = 1500,
            Rank = 0,
            Premium = false,
            Gold = 0,
            Level = 1,
            Experience = 0,
        };
    }

    public GetPlayerResponse GetPlayersResponseProtoFromDto()
    {
        return new GetPlayerResponse()
        {
            Id = Id,
            Email = Email,
            Password = Password,
            Nickname = Nickname,
            Role = Role,
            LastLogin = LastLogin.ToTimestamp(),
            AvatarId = AvatarId,
            FrameId = FrameId,
            Games = Games,
            Wins = Wins,
            Loses = Loses,
            Mmr = Mmr,
            Rank = Rank,
            Premium = Premium,
            Gold = Gold,
            Level = Level,
            Experience = Experience
        };
    }
}
