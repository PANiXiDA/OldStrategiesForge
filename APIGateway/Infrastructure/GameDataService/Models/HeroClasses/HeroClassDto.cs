using APIGateway.Infrastructure.GameDataService.Models.Abilities;
using GameData.Entities.Gen;
using System.ComponentModel.DataAnnotations;

namespace APIGateway.Infrastructure.GameDataService.Models.HeroClasses;

public class HeroClassDto
{
    [Required(ErrorMessage = "Id is required.")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required.")]
    public string Description { get; set; } = string.Empty;

    public List<AbilityDto> Abilities { get; set; } = new();

    public static HeroClassDto FromEntity(HeroClass obj)
    {
        return new HeroClassDto
        {
            Id = obj.Id,
            Name = obj.Name,
            Description = obj.Description,
            Abilities = AbilityDto.FromEntitiesList(obj.Abilities)
        };
    }

    public static HeroClass ToEntity(HeroClassDto obj)
    {
        var entity = new HeroClass
        {
            Id = obj.Id,
            Name = obj.Name,
            Description = obj.Description
        };

        entity.Abilities.AddRange(AbilityDto.ToEntitiesList(obj.Abilities));

        return entity;
    }

    public static List<HeroClassDto> FromEntitiesList(IEnumerable<HeroClass> list)
    {
        return list?.Select(FromEntity).Where(item => item != null).ToList() ?? new List<HeroClassDto>();
    }

    public static List<HeroClass> ToEntitiesList(IEnumerable<HeroClassDto> list)
    {
        return list?.Select(ToEntity).Where(item => item != null).ToList() ?? new List<HeroClass>();
    }
}
