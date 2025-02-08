using System.ComponentModel.DataAnnotations.Schema;

namespace GameDataService.DAL.DbModels.Models;

public class FactionAndAbilityScope
{
    [Column("id")]
    public int Id { get; set; }

    [Column("faction_id")]
    public int FactionId { get; set; }

    [Column("ability_id")]
    public int AbilityId { get; set; }

    public virtual Faction? Faction { get; set; }
    public virtual Ability? Ability { get; set; }
}
