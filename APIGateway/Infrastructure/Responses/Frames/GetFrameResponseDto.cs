using APIGateway.Infrastructure.Responses.Avatars;
using Profile.Frames.Gen;

namespace APIGateway.Infrastructure.Responses.Frames;

public class GetFrameResponseDto
{
    public int Id { get; set; }
    public string S3Path { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int NecessaryMmr { get; set; }
    public int NecessaryGames { get; set; }
    public int NecessaryWins { get; set; }
    public bool Available { get; set; }
    public string FileName { get; set; } = string.Empty;

    public static GetFrameResponseDto GetFrameResponseFromProtoToDto(GetFrameResponse getAvatarResponse)
    {
        return new GetFrameResponseDto()
        {
            Id = getAvatarResponse.Id,
            S3Path = getAvatarResponse.S3Path,
            Name = getAvatarResponse.Name,
            Description = getAvatarResponse.Description,
            NecessaryMmr = getAvatarResponse.NecessaryMmr,
            NecessaryGames = getAvatarResponse.NecessaryGames,
            NecessaryWins = getAvatarResponse.NecessaryWins,
            Available = getAvatarResponse.Available,
            FileName = getAvatarResponse.FileName
        };
    }
}
