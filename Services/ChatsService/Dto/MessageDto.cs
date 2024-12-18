using BaseBL.Models;

namespace ChatsService.Dto;

public class MessageDto : BaseEntity<Guid>
{
    public Guid ChatId { get; set; }
    public int SenderId { get; set; }
    public string Content { get; set; } = string.Empty;
    public bool IsRead { get; set; } = false;

    public MessageDto(
        Guid id,
        DateTime createdAt,
        DateTime updateAt,
        DateTime? deletedAt,
        Guid chatId,
        int senderId,
        string content,
        bool isRead) : base(id, createdAt, updateAt, deletedAt)
    {
        ChatId = chatId;
        SenderId = senderId;
        Content = content;
        IsRead = isRead;
    }

    public MessageDto(
        Guid chatId,
        int senderId,
        string content,
        bool isRead) : base(Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow, null)
    {
        ChatId = chatId;
        SenderId = senderId;
        Content = content;
        IsRead = isRead;
    }
}
