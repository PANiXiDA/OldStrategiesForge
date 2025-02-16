using APIGateway.Infrastructure.GameDataService.Models.Abilities;
using APIGateway.Infrastructure.GameDataService.Models.Competencies;
using APIGateway.Infrastructure.GameDataService.Models.Spells;
using APIGateway.Infrastructure.GameDataService.Models.Subfactions;
using GameData.Entities.Gen;
using GameData.Enums.Gen;
using System.ComponentModel.DataAnnotations;

namespace APIGateway.Infrastructure.GameDataService.Models.Skills;

public class SkillDto
{
    [Required(ErrorMessage = "Id is required.")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required.")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "SkillType is required.")]
    public SkillType SkillType { get; set; }

    public int? CompetenceId { get; set; }

    public int? SubfactionId { get; set; }

    public int? AbilityId { get; set; }

    public CompetenceDto? Competence { get; set; }

    public SubfactionDto? Subfaction { get; set; }

    public AbilityDto? Ability { get; set; }

    public List<SkillDto> RequiredSkills { get; set; } = new();

    public List<SkillDto> DependentSkills { get; set; } = new();

    public List<SpellDto> Spells { get; set; } = new();

    public static SkillDto FromEntity(Skill obj)
    {
        return new SkillDto
        {
            Id = obj.Id,
            Name = obj.Name,
            Description = obj.Description,
            SkillType = obj.SkillType,
            CompetenceId = obj.CompetenceId,
            SubfactionId = obj.SubfactionId,
            AbilityId = obj.AbilityId,
            Competence = obj.Competence != null ? CompetenceDto.FromEntity(obj.Competence) : null,
            Subfaction = obj.Subfaction != null ? SubfactionDto.FromEntity(obj.Subfaction) : null,
            Ability = obj.Ability != null ? AbilityDto.FromEntity(obj.Ability) : null,
            RequiredSkills = FromEntitiesList(obj.RequiredSkills),
            DependentSkills = FromEntitiesList(obj.DependentSkills),
            Spells = SpellDto.FromEntitiesList(obj.Spells)
        };
    }

    public static Skill ToEntity(SkillDto obj)
    {
        var entity = new Skill
        {
            Id = obj.Id,
            Name = obj.Name,
            Description = obj.Description,
            SkillType = obj.SkillType,
            CompetenceId = obj.CompetenceId,
            SubfactionId = obj.SubfactionId,
            AbilityId = obj.AbilityId,
            Competence = obj.Competence != null ? CompetenceDto.ToEntity(obj.Competence) : null,
            Subfaction = obj.Subfaction != null ? SubfactionDto.ToEntity(obj.Subfaction) : null,
            Ability = obj.Ability != null ? AbilityDto.ToEntity(obj.Ability) : null,
        };

        entity.RequiredSkills.AddRange(ToEntitiesList(obj.RequiredSkills));
        entity.DependentSkills.AddRange(ToEntitiesList(obj.DependentSkills));
        entity.Spells.AddRange(SpellDto.ToEntitiesList(obj.Spells));

        return entity;
    }

    public static List<SkillDto> FromEntitiesList(IEnumerable<Skill> list)
    {
        return list?.Select(FromEntity).Where(item => item != null).ToList() ?? new List<SkillDto>();
    }

    public static List<Skill> ToEntitiesList(IEnumerable<SkillDto> list)
    {
        return list?.Select(ToEntity).Where(item => item != null).ToList() ?? new List<Skill>();
    }
}
