using System.ComponentModel.DataAnnotations.Schema;

namespace GameDataService.DAL.DbModels.Models;

public class SkillAndSpellScope
{
    [Column("id")]
    public int Id { get; set; }

    [Column("skill_id")]
    public int SkillId { get; set; }

    [Column("spell_id")]
    public int SpellId { get; set; }

    public virtual Skill? Skill { get; set; }
    public virtual Spell? Spell { get; set; }
}
