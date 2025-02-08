using System.ComponentModel.DataAnnotations.Schema;

namespace GameDataService.DAL.DbModels.Models;

public class SpellAndAbilityScope
{
    [Column("id")]
    public int Id { get; set; }

    [Column("spell_id")]
    public int SpellId { get; set; }

    [Column("ability_id")]
    public int AbilityId { get; set; }

    public virtual Spell? Spell { get; set; }
    public virtual Ability? Ability { get; set; }
}
