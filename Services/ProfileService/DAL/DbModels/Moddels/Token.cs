using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProfileService.DAL.DbModels.Models;

public class Token
{
    [Key]
    [Column("id")]
    [Required]
    public int Id { get; set; }

    [ForeignKey("Player")]
    [Column("player_id")]
    [Required]
    public int PlayerId { get; set; }

    [Column("refresh_token")]
    [Required]
    public string RefreshToken { get; set; } = string.Empty;

    [Column("refresh_token_exp")]
    [Required]
    public DateTime RefreshTokenExp { get; set; }

    public virtual Player? Player { get; set; }
}
