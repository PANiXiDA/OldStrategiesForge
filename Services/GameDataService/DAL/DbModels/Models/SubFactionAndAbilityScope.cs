using System.ComponentModel.DataAnnotations.Schema;

namespace GameDataService.DAL.DbModels.Models;

public class SubFactionAndAbilityScope
{
    [Column("id")]
    public int Id { get; set; }

    [Column("subfaction_id")]
    public int SubFactionId { get; set; }

    [Column("ability_id")]
    public int AbilityId { get; set; }

    public virtual SubFaction? SubFaction { get; set; }
    public virtual Ability? Ability { get; set; }
}
