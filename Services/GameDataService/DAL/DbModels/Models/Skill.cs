using Common.Enums;
using GameData.Enums.Gen;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameDataService.DAL.DbModels.Models;

public class Skill
{
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Column("description")]
    public string Description { get; set; } = string.Empty;

    [Column("skill_type")]
    public SkillType SkillType { get; set; }

    [Column("competence_id")]
    public int? CompetenceId { get; set; }

    [Column("subfaction_id")]
    public int? SubfactionId { get; set; }

    [Column("ability_id")]
    public int? AbilityId { get; set; }

    public virtual Competence? Competence { get; set; }
    public virtual Subfaction? Subfaction { get; set; }
    public virtual Ability? Ability { get; set; }

    public virtual ICollection<SkillDependency> RequiredSkillDependencies { get; set; } = new HashSet<SkillDependency>();
    public virtual ICollection<SkillDependency> DependentSkillDependencies { get; set; } = new HashSet<SkillDependency>();
    public virtual ICollection<SkillAndSpellScope> SkillAndSpellScopes { get; set; } = new HashSet<SkillAndSpellScope>();

    [NotMapped]
    public IEnumerable<Skill> RequiredSkills => RequiredSkillDependencies
        .Where(item => item.RequiredSkill != null)
        .Select(item => item.RequiredSkill!);

    [NotMapped]
    public IEnumerable<Skill> DependentSkills => DependentSkillDependencies
        .Where(item => item.Skill != null)
        .Select(item => item.Skill!);

    [NotMapped]
    public IEnumerable<Spell> Spells => SkillAndSpellScopes
        .Where(item => item.Spell != null)
        .Select(item => item.Spell!);
}
