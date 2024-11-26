using Profile.Avatar.Gen;

namespace APIGateway.Infrastructure.Responses.Avatars;

public class GetAvatarResponseDto
{
    public string S3Path { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int NecessaryMmr { get; set; }
    public int NecessaryGames { get; set; }
    public int NecessaryWins { get; set; }
    public bool Available { get; set; }

    public static GetAvatarResponseDto GetAvatarResponseFromProtoToDto(GetAvatarResponse getAvatarResponse)
    {
        return new GetAvatarResponseDto()
        {
            S3Path = getAvatarResponse.S3Path,
            Name = getAvatarResponse.Name,
            Description = getAvatarResponse.Description,
            NecessaryMmr = getAvatarResponse.NecessaryMmr,
            NecessaryGames = getAvatarResponse.NecessaryGames,
            NecessaryWins = getAvatarResponse.NecessaryWins,
            Available = getAvatarResponse.Available
        };
    }
}