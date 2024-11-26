using BaseDAL.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProfileService.DAL.DbModels.Models;

public partial class Avatar : BasePostgresDbModel
{
    [Column("s3_path")]
    [Required]    
    public string S3Path { get; set; } = string.Empty;

    [Column("name")]
    [Required]
    public string Name { get; set; } = string.Empty;

    [Column("description")]
    [Required]
    public string Description { get; set; } = string.Empty;

    [Column("necessary_mmr")]
    [Required]
    public int NecessaryMmr { get; set; }

    [Column("necessary_games")]
    [Required]
    public int NecessaryGames { get; set; }

    [Column("necessary_wins")]
    [Required]
    public int NecessaryWins { get; set; }

    [Column("available")]
    [Required]
    public bool Available { get; set; }
}
