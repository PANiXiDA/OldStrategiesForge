using BaseBL.Models;
using Common.Enums;

namespace GamesService.Dto;

public class GameDto : BaseEntity<Guid>
{
    public GameType GameType { get; set; }
    public int? WinnerId { get; set; }

    public List<SessionDto>? Sessions { get; set; }

    public GameDto(
        Guid id,
        DateTime createdAt,
        DateTime updateAt,
        DateTime? deletedAt,
        GameType gameType,
        int? winnerId = null) : base(id, createdAt, updateAt, deletedAt)
    {
        GameType = gameType;
        WinnerId = winnerId;
    }

    public GameDto(
        GameType gameType,
        int? winnerId = null) : base(Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow, null)
    {
        GameType = gameType;
        WinnerId = winnerId;
    }
}