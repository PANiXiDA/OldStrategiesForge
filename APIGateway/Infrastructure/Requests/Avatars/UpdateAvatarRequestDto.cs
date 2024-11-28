using Profile.Avatar.Gen;
using System.ComponentModel.DataAnnotations;

namespace APIGateway.Infrastructure.Requests.Avatars;

public class UpdateAvatarRequestDto
{
    [Required(ErrorMessage = "Id is required.")]
    public int Id { get; set; }

    [Required(ErrorMessage = "S3Path is required.")]
    public string S3Path { get; set; } = string.Empty;

    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required.")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "NecessaryMmr is required.")]
    public int NecessaryMmr { get; set; }

    [Required(ErrorMessage = "NecessaryGames is required.")]
    public int NecessaryGames { get; set; }

    [Required(ErrorMessage = "NecessaryWins is required.")]
    public int NecessaryWins { get; set; }

    [Required(ErrorMessage = "Available is required.")]
    public bool Available { get; set; }
    public UpdateAvatarRequest UpdateAvatarRequestDtoToProto()
    {
        return new UpdateAvatarRequest()
        {
            Id = Id,
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
