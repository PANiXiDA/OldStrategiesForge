using BaseDAL.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ChatsService.DAL.DbModels.Models;

public class PrivateChat : BaseDbModel<Guid>
{
    [Column("player1_id")]
    [Required]
    public int Player1Id { get; set; }

    [Column("player2_id")]
    [Required]
    public int Player2Id { get; set; }
}
