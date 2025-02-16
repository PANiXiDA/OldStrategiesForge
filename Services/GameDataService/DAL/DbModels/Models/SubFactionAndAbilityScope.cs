using System.ComponentModel.DataAnnotations.Schema;

namespace GameDataService.DAL.DbModels.Models;

public class SubfactionAndAbilityScope
{
    [Column("id")]
    public int Id { get; set; }

    [Column("subfaction_id")]
    public int SubfactionId { get; set; }

    [Column("ability_id")]
    public int AbilityId { get; set; }

    public virtual Subfaction? Subfaction { get; set; }
    public virtual Ability? Ability { get; set; }
}
