using Profile.Avatar.Gen;

namespace ProfileService.Dto;

public class AvatarsDto
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string S3Path { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int NecessaryMmr { get; set; }
    public int NecessaryGames { get; set; }
    public int NecessaryWins { get; set; }
    public bool Available { get; set; }

    public AvatarsDto(
        int id,
        string s3Path,
        string name,
        string description,
        int necessaryMmr,
        int necessaryGames,
        int necessaryWins,
        bool available)
    {
        Id = 0;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        DeletedAt = null;
        S3Path = s3Path;
        Name = name;
        Description = description;
        NecessaryMmr = necessaryMmr;
        NecessaryGames = necessaryGames;
        NecessaryWins = necessaryWins;
        Available = available;
    }

    public GetAvatarResponse AvatarsProtoGetFromDto()
    {
        return new GetAvatarResponse()
        {
            S3Path = S3Path,
            Name = Name,
            Description = Description,
            NecessaryMmr = NecessaryMmr,
            NecessaryGames = NecessaryGames,
            NecessaryWins = NecessaryWins,
            Available = Available
        };
    }
}
