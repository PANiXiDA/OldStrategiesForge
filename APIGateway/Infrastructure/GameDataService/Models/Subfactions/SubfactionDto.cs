using APIGateway.Infrastructure.GameDataService.Models.Abilities;
using APIGateway.Infrastructure.GameDataService.Models.Factions;
using APIGateway.Infrastructure.GameDataService.Models.Skills;
using GameData.Entities.Gen;
using System.ComponentModel.DataAnnotations;

namespace APIGateway.Infrastructure.GameDataService.Models.Subfactions;

public class SubfactionDto
{
    [Required(ErrorMessage = "Id is required.")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required.")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "FactionId is required.")]
    public int FactionId { get; set; }

    public FactionDto? Faction { get; set; }

    public List<SkillDto> Skills { get; set; } = new();

    public List<AbilityDto> Abilities { get; set; } = new();

    public static SubfactionDto FromEntity(Subfaction obj)
    {
        return new SubfactionDto
        {
            Id = obj.Id,
            Name = obj.Name,
            Description = obj.Description,
            FactionId = obj.FactionId,
            Faction = obj.Faction != null ? FactionDto.FromEntity(obj.Faction) : null,
            Skills = SkillDto.FromEntitiesList(obj.Skills),
            Abilities = AbilityDto.FromEntitiesList(obj.Abilities)
        };
    }

    public static Subfaction ToEntity(SubfactionDto obj)
    {
        var entity = new Subfaction
        {
            Id = obj.Id,
            Name = obj.Name,
            Description = obj.Description,
            FactionId = obj.FactionId,
            Faction = obj.Faction != null ? FactionDto.ToEntity(obj.Faction) : null
        };

        entity.Skills.AddRange(SkillDto.ToEntitiesList(obj.Skills));
        entity.Abilities.AddRange(AbilityDto.ToEntitiesList(obj.Abilities));

        return entity;
    }

    public static List<SubfactionDto> FromEntitiesList(IEnumerable<Subfaction> list)
    {
        return list?.Select(FromEntity).Where(item => item != null).ToList() ?? new List<SubfactionDto>();
    }

    public static List<Subfaction> ToEntitiesList(IEnumerable<SubfactionDto> list)
    {
        return list?.Select(ToEntity).Where(item => item != null).ToList() ?? new List<Subfaction>();
    }
}
