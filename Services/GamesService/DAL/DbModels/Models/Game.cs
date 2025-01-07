using BaseDAL.Models;
using Common.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GamesService.DAL.DbModels.Models;

public class Game : BaseDbModel<Guid>
{
    [Column("game_type")]
    [Required]
    public GameType GameType { get; set; }

    [Column("winner_id")]
    public int? WinnerId { get; set; }

    public virtual ICollection<Session> Sessions { get; set; }

    public Game()
    {
        Sessions = new HashSet<Session>();
    }
}