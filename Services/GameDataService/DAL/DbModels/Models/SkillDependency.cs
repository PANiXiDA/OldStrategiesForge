using System.ComponentModel.DataAnnotations.Schema;

namespace GameDataService.DAL.DbModels.Models;

public class SkillDependency
{
    [Column("id")]
    public int Id { get; set; }

    [Column("skill_id")]
    public int SkillId { get; set; }

    [Column("required_skill_id")]
    public int RequiredSkillId { get; set; }

    public virtual Skill? Skill { get; set; }
    public virtual Skill? RequiredSkill { get; set; }
}
