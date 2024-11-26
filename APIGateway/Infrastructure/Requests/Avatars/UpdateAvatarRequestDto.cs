﻿using Profile.Avatar.Gen;
using System.ComponentModel.DataAnnotations;

namespace APIGateway.Infrastructure.Requests.Avatars;

public class UpdateAvatarRequestDto
{
    [Required]
    public int Id { get; set; }

    [Required]
    public string S3Path { get; set; } = string.Empty;

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    public int NecessaryMmr { get; set; }

    [Required]
    public int NecessaryGames { get; set; }

    [Required]
    public int NecessaryWins { get; set; }

    [Required]
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
