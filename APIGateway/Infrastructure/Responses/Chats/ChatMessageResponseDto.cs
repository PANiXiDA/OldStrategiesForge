using Global.Chat.Gen;

namespace APIGateway.Infrastructure.Responses.Chats;

public class ChatMessageResponseDto
{
    public string MessageId { get; set; } = string.Empty;
    public int SenderId { get; set; }
    public string SenderNickname { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string AvatarS3Path { get; set; } = string.Empty;
    public string AvatarFileName { get; set; } = string.Empty;
    public string FrameS3Path { get; set; } = string.Empty;
    public string FrameFileName { get; set; } = string.Empty;
    public DateTime TimeSending { get; set; }

    public static ChatMessageResponseDto ChatMessageResponseFromProtoToDto(ChatMessageResponse chatMessageResponse)
    {
        return new ChatMessageResponseDto()
        {
            MessageId = chatMessageResponse.MessageId,
            SenderId = chatMessageResponse.SenderId,
            SenderNickname = chatMessageResponse.SenderNickname,
            Content = chatMessageResponse.Content,
            AvatarS3Path = chatMessageResponse.AvatarS3Path,
            AvatarFileName = chatMessageResponse.AvatarFileName,
            FrameS3Path = chatMessageResponse.FrameS3Path,
            FrameFileName = chatMessageResponse.FrameFileName,
            TimeSending = chatMessageResponse.TimeSending.ToDateTime()
        };
    }
}
