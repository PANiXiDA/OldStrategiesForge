using Profile.Frames.Gen;

namespace ProfileService.Dto;

public class FramesDto
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
    public string FileName
    {
        get
        {
            var fileNameWithParams = S3Path.Substring(S3Path.LastIndexOf('/') + 1);
            return fileNameWithParams.Contains('?')
                ? fileNameWithParams.Substring(0, fileNameWithParams.IndexOf('?'))
                : fileNameWithParams;
        }
        set
        {
            var basePath = S3Path.Contains('?')
                ? S3Path.Substring(0, S3Path.IndexOf('?'))
                : S3Path;
            S3Path = $"{basePath.Substring(0, basePath.LastIndexOf('/') + 1)}{value}";
        }
    }


    public FramesDto(
        int id,
        string s3Path,
        string name,
        string description,
        int necessaryMmr,
        int necessaryGames,
        int necessaryWins,
        bool available)
    {
        Id = id;
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

    public GetFrameResponse AvatarsProtoGetFromDto()
    {
        return new GetFrameResponse()
        {
            Id = Id,
            S3Path = S3Path,
            Name = Name,
            Description = Description,
            NecessaryMmr = NecessaryMmr,
            NecessaryGames = NecessaryGames,
            NecessaryWins = NecessaryWins,
            Available = Available,
            FileName = FileName
        };
    }
}
