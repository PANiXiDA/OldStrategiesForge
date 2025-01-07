using BaseDAL.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GamesService.DAL.DbModels.Models;

public class Session : BaseDbModel<Guid>
{
    [Column("player_id")]
    [Required]
    public int PlayerId { get; set; }

    [Column("game_id")]
    [Required]
    public Guid GameId { get; set; }

    [Column("is_active")]
    [Required]
    public bool IsActive { get; set; }

    public virtual Game? Game { get; set; }
}