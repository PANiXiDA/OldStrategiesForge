using APIGateway.Infrastructure.GameDataService.Models.Abilities;
using APIGateway.Infrastructure.GameDataService.Models.Skills;
using GameData.Entities.Gen;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace APIGateway.Infrastructure.GameDataService.Models.Spells;

public class SpellDto
{
    [Required(ErrorMessage = "Id is required.")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required.")]
    public string Description { get; set; } = string.Empty;

    public int? RequiredSkillId { get; set; }

    [ValidateNever]
    public SkillDto? RequiredSkill { get; set; }

    [ValidateNever]
    public List<AbilityDto> Abilities { get; set; } = new();

    public static SpellDto FromEntity(Spell obj)
    {
        return new SpellDto
        {
            Id = obj.Id,
            Name = obj.Name,
            Description = obj.Description,
            RequiredSkillId = obj.RequiredSkillId,
            RequiredSkill = SkillDto.FromEntity(obj.RequiredSkill),
            Abilities = AbilityDto.FromEntitiesList(obj.Abilities)
        };
    }

    public static Spell ToEntity(SpellDto obj)
    {
        var entity = new Spell
        {
            Id = obj.Id,
            Name = obj.Name,
            Description = obj.Description,
            RequiredSkillId = obj.RequiredSkillId,
            RequiredSkill = obj.RequiredSkill != null ? SkillDto.ToEntity(obj.RequiredSkill) : null
        };

        entity.Abilities.AddRange(AbilityDto.ToEntitiesList(obj.Abilities));

        return entity;
    }

    public static List<SpellDto> FromEntitiesList(IEnumerable<Spell> list)
    {
        return list?.Select(FromEntity).Where(item => item != null).ToList() ?? new List<SpellDto>();
    }

    public static List<Spell> ToEntitiesList(IEnumerable<SpellDto> list)
    {
        return list?.Select(ToEntity).Where(item => item != null).ToList() ?? new List<Spell>();
    }
}
