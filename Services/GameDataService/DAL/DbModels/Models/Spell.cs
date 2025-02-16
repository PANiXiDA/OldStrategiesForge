using System.ComponentModel.DataAnnotations.Schema;

namespace GameDataService.DAL.DbModels.Models;

public class Spell
{
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Column("description")]
    public string Description { get; set; } = string.Empty;

    [Column("required_skill_id")]
    public int? RequiredSkillId { get; set; }

    public virtual Skill? RequiredSkill { get; set; }

    public virtual ICollection<SpellAndAbilityScope> SpellAndAbilityScopes { get; set; } = new HashSet<SpellAndAbilityScope>();

    [NotMapped]
    public IEnumerable<Ability> Abilities => SpellAndAbilityScopes
        .Where(item => item.Ability != null)
        .Select(item => item.Ability!);
}
