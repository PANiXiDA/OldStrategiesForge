using APIGateway.Infrastructure.GameDataService.Models.Abilities;
using APIGateway.Infrastructure.GameDataService.Models.Artefacts;
using APIGateway.Infrastructure.GameDataService.Models.HeroClasses;
using APIGateway.Infrastructure.GameDataService.Models.Subfactions;
using GameData.Entities.Gen;
using System.ComponentModel.DataAnnotations;

namespace APIGateway.Infrastructure.GameDataService.Models.Heroes;

public class HeroDto
{
    [Required(ErrorMessage = "Id is required.")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required.")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Attack is required.")]
    public int Attack { get; set; }

    [Required(ErrorMessage = "Defence is required.")]
    public int Defence { get; set; }

    [Required(ErrorMessage = "MinDamage is required.")]
    public int MinDamage { get; set; }

    [Required(ErrorMessage = "MaxDamage is required.")]
    public int MaxDamage { get; set; }

    [Required(ErrorMessage = "Initiative is required.")]
    public double Initiative { get; set; }

    [Required(ErrorMessage = "Morale is required.")]
    public int Morale { get; set; }

    [Required(ErrorMessage = "Luck is required.")]
    public int Luck { get; set; }

    [Required(ErrorMessage = "SubfactionId is required.")]
    public int SubfactionId { get; set; }

    [Required(ErrorMessage = "HeroClassId is required.")]
    public int HeroClassId { get; set; }

    public SubfactionDto? Subfaction { get; set; }

    public HeroClassDto? HeroClass { get; set; }

    public List<AbilityDto> Abilities { get; set; } = new();

    public List<ArtefactDto> Artefacts { get; set; } = new();

    public static HeroDto FromEntity(Hero obj)
    {
        return new HeroDto
        {
            Id = obj.Id,
            Name = obj.Name,
            Description = obj.Description,
            Attack = obj.Attack,
            Defence = obj.Defence,
            MinDamage = obj.MinDamage,
            MaxDamage = obj.MaxDamage,
            Initiative = obj.Initiative,
            Morale = obj.Morale,
            Luck = obj.Luck,
            SubfactionId = obj.SubfactionId,
            HeroClassId = obj.HeroClassId,
            Subfaction = obj.Subfaction != null ? SubfactionDto.FromEntity(obj.Subfaction) : null,
            HeroClass = obj.HeroClass != null ? HeroClassDto.FromEntity(obj.HeroClass) : null,
            Abilities = AbilityDto.FromEntitiesList(obj.Abilities),
            Artefacts = ArtefactDto.FromEntitiesList(obj.Artefacts)
        };
    }

    public static Hero ToEntity(HeroDto obj)
    {
        var entity = new Hero
        {
            Id = obj.Id,
            Name = obj.Name,
            Description = obj.Description,
            Attack = obj.Attack,
            Defence = obj.Defence,
            MinDamage = obj.MinDamage,
            MaxDamage = obj.MaxDamage,
            Initiative = obj.Initiative,
            Morale = obj.Morale,
            Luck = obj.Luck,
            SubfactionId = obj.SubfactionId,
            HeroClassId = obj.HeroClassId,
            Subfaction = obj.Subfaction != null ? SubfactionDto.ToEntity(obj.Subfaction) : null,
            HeroClass = obj.HeroClass != null ? HeroClassDto.ToEntity(obj.HeroClass) : null
        };

        entity.Abilities.AddRange(AbilityDto.ToEntitiesList(obj.Abilities));
        entity.Artefacts.AddRange(ArtefactDto.ToEntitiesList(obj.Artefacts));

        return entity;
    }

    public static List<HeroDto> FromEntitiesList(IEnumerable<Hero> list)
    {
        return list?.Select(FromEntity).Where(item => item != null).ToList() ?? new List<HeroDto>();
    }

    public static List<Hero> ToEntitiesList(IEnumerable<HeroDto> list)
    {
        return list?.Select(ToEntity).Where(item => item != null).ToList() ?? new List<Hero>();
    }
}
