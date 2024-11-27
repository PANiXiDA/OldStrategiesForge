using BaseDAL.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProfileService.DAL.DbModels.Models;

public partial class Player : BasePostgresDbModel
{
    [Column("email")]
    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    [Column("password")]
    [Required]
    public string Password { get; set; } = string.Empty;

    [Column("nickname")]
    [Required]
    [MaxLength(20)]
    public string Nickname { get; set; } = string.Empty;

    [Column("confirmed")]
    public bool Confirmed { get; set; }

    [Column("blocked")]
    public bool Blocked { get; set; }

    [Column("role")]
    public int Role { get; set; }

    [Column("last_login")]
    public DateTime LastLogin { get; set; }

    [Column("avatar_id")]
    public int AvatarId { get; set; }

    [Column("frame_id")]
    public int FrameId { get; set; }

    [Column("games")]
    public int Games { get; set; }

    [Column("wins")]
    public int Wins { get; set; }

    [Column("loses")]
    public int Loses { get; set; }

    [Column("mmr")]
    public int Mmr { get; set; }

    [Column("rank")]
    public int Rank { get; set; }

    [Column("premium")]
    public bool Premium { get; set; }

    [Column("gold")]
    public int Gold { get; set; }

    [Column("level")]
    public int Level { get; set; }

    [Column("experience")]
    public int Experience { get; set; }

    public virtual Avatar? Avatar { get; set; }
}
