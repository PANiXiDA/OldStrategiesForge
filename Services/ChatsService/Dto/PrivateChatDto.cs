using BaseBL.Models;
using Common.Enums;

namespace ChatsService.Dto;

public class PrivateChatDto : BaseEntity<Guid>
{
    public int Player1Id { get; set; }
    public int Player2Id { get; set; }

    public PrivateChatDto(
        Guid id,
        DateTime createdAt,
        DateTime updateAt,
        DateTime? deletedAt,
        int player1Id,
        int player2Id) : base(id, createdAt, updateAt, deletedAt)
    {
        Player1Id = player1Id;
        Player2Id = player2Id;
    }

    public PrivateChatDto(
        int player1Id,
        int player2Id) : base(Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow, null)
    {
        Player1Id = player1Id;
        Player2Id = player2Id;
    }
}
