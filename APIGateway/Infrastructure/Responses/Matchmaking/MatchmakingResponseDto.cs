using APIGateway.Infrastructure.Responses.Players;
using Matchmaking.Gen;

namespace APIGateway.Infrastructure.Responses.Matchmaking;

public class MatchmakingResponseDto
{
    public string GameId { get; set; } = string.Empty;
    public int OpponentId { get; set; }
    public string OpponentNickname { get; set; } = string.Empty;
    public int Mmr { get; set; }
    public int Rank { get; set; }
    public int Level { get; set; }
    public AvatarDto Avatar { get; set; } = new();
    public FrameDto Frame { get; set; } = new();

    public static MatchmakingResponseDto MatchmakingResponseFromProtoToDto(MatchmakingResponse matchmakingResponse)
    {
        return new MatchmakingResponseDto()
        {
            GameId = matchmakingResponse.GameId,
            OpponentId = matchmakingResponse.OpponentId,
            OpponentNickname = matchmakingResponse.OpponentNickname,
            Mmr = matchmakingResponse.Mmr,
            Rank = matchmakingResponse.Rank,
            Level = matchmakingResponse.Level,
            Avatar = new AvatarDto()
            {
                Id = matchmakingResponse.Avatar.Id,
                S3Path = matchmakingResponse.Avatar.S3Path,
                Description = matchmakingResponse.Avatar.Description,
                Name = matchmakingResponse.Avatar.Name,
                FileName = matchmakingResponse.Avatar.FileName
            },
            Frame = new FrameDto()
            {
                Id = matchmakingResponse.Frame.Id,
                S3Path = matchmakingResponse.Frame.S3Path,
                Description = matchmakingResponse.Frame.Description,
                Name = matchmakingResponse.Frame.Name,
                FileName = matchmakingResponse.Frame.FileName
            }
        };
    }
}
