using BaseBL.Models;

namespace GamesService.Dto;

public class SessionDto : BaseEntity<Guid>
{
    public int PlayerId { get; set; }
    public Guid GameId { get; set; }
    public bool IsActive { get; set; }

    public GameDto? Game { get; set; }

    public SessionDto(
        Guid id,
        DateTime createdAt,
        DateTime updateAt,
        DateTime? deletedAt,
        int playerId,
        Guid gameId,
        bool isActive) : base(id, createdAt, updateAt, deletedAt)
    {
        PlayerId = playerId;
        GameId = gameId;
        IsActive = isActive;
    }

    public SessionDto(
        int playerId,
        Guid gameId,
        bool isActive) : base(Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow, null)
    {
        PlayerId = playerId;
        GameId = gameId;
        IsActive = isActive;
    }
}