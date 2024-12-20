using BaseBL.Models;
using System.Threading;

namespace ChatsService.Dto;

public class MessageDto : BaseEntity<Guid>
{
    public Guid ChatId { get; set; }
    public string Content { get; set; }
    public int SenderId { get; set; }
    public string SenderNickname { get; set; }
    public string AvatarS3Path { get; set; }
    public string FrameS3Path { get; set; }
    public bool IsRead { get; set; }
    public string? AvatarFileName { get; set; }
    public string? FrameFileName { get; set; }

    public MessageDto() : base(Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow, null) { }

    public MessageDto(
        Guid id,
        DateTime createdAt,
        DateTime updateAt,
        DateTime? deletedAt,
        Guid chatId,
        string content,
        int senderId,
        string senderNickname,
        string avatarS3Path,
        string frameS3Path,
        bool isRead = false) : base(id, createdAt, updateAt, deletedAt)
    {
        ChatId = chatId;
        Content = content;
        SenderId = senderId;
        SenderNickname = senderNickname;
        AvatarS3Path = avatarS3Path;
        FrameS3Path = frameS3Path;
        IsRead = isRead;
    }

    public MessageDto(
        Guid chatId,
        string content,
        int senderId,
        string senderNickname,
        string avatarS3Path,
        string frameS3Path,
        bool isRead = false) : base(Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow, null)
    {
        ChatId = chatId;
        Content = content;
        SenderId = senderId;
        SenderNickname = senderNickname;
        AvatarS3Path = avatarS3Path;
        FrameS3Path = frameS3Path;
        IsRead = isRead;
    }
}
