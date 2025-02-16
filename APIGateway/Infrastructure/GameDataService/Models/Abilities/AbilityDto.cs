using APIGateway.Infrastructure.GameDataService.Models.Effects;
using GameData.Entities.Gen;
using System.ComponentModel.DataAnnotations;

namespace APIGateway.Infrastructure.GameDataService.Models.Abilities;

public class AbilityDto
{
    [Required(ErrorMessage = "Id is required.")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required.")]
    public string Description { get; set; } = string.Empty;

    public List<EffectDto> Effects { get; set; } = new();

    public static AbilityDto FromEntity(Ability obj)
    {
        return new AbilityDto
        {
            Id = obj.Id,
            Name = obj.Name,
            Description = obj.Description,
            Effects = EffectDto.FromEntitiesList(obj.Effects)
        };
    }

    public static Ability ToEntity(AbilityDto obj)
    {
        var entity = new Ability
        {
            Id = obj.Id,
            Name = obj.Name,
            Description = obj.Description,
        };

        entity.Effects.AddRange(EffectDto.ToEntitiesList(obj.Effects));

        return entity;
    }

    public static List<AbilityDto> FromEntitiesList(IEnumerable<Ability> list)
    {
        return list?.Select(FromEntity).Where(item => item != null).ToList() ?? new List<AbilityDto>();
    }

    public static List<Ability> ToEntitiesList(IEnumerable<AbilityDto> list)
    {
        return list?.Select(ToEntity).Where(item => item != null).ToList() ?? new List<Ability>();
    }
}
