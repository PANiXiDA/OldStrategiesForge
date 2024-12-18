using BaseBL.Models;
using Common.Enums;

namespace ChatsService.Dto;

public class ChatDto : BaseEntity<Guid>
{
    public ChatType ChatType { get; set; }
    public string? Name { get; set; }

    public ChatDto(
        Guid id,
        DateTime createdAt,
        DateTime updateAt,
        DateTime? deletedAt,
        ChatType chatType,
        string? name) : base(id, createdAt, updateAt, deletedAt)
    {
        ChatType = chatType;
        Name = name;
    }

    public ChatDto(
        ChatType chatType,
        string? name) : base(Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow, null)
    {
        ChatType = chatType;
        Name = name;
    }
}
