using System.ComponentModel.DataAnnotations.Schema;

namespace GameDataService.DAL.DbModels.Models;

public class AbilityAndEffectScope
{
    [Column("id")]
    public int Id { get; set; }

    [Column("ability_id")]
    public int AbilityId { get; set; }

    [Column("effect_id")]
    public int EffectId { get; set; }

    public virtual Ability? Ability { get; set; }
    public virtual Effect? Effect { get; set; }
}
