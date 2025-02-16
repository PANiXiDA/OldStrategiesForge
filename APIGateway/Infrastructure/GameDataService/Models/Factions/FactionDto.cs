using APIGateway.Infrastructure.GameDataService.Models.Abilities;
using GameData.Entities.Gen;
using System.ComponentModel.DataAnnotations;

namespace APIGateway.Infrastructure.GameDataService.Models.Factions;

public class FactionDto
{
    [Required(ErrorMessage = "Id is required.")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required.")]
    public string Description { get; set; } = string.Empty;

    public List<AbilityDto> Abilities { get; set; } = new();

    public static FactionDto FromEntity(Faction obj)
    {
        return new FactionDto
        {
            Id = obj.Id,
            Name = obj.Name,
            Description = obj.Description,
            Abilities = AbilityDto.FromEntitiesList(obj.Abilities)
        };
    }

    public static Faction ToEntity(FactionDto obj)
    {
        var entity = new Faction
        {
            Id = obj.Id,
            Name = obj.Name,
            Description = obj.Description,
        };

        entity.Abilities.AddRange(AbilityDto.ToEntitiesList(obj.Abilities));

        return entity;
    }

    public static List<FactionDto> FromEntitiesList(IEnumerable<Faction> list)
    {
        return list?.Select(FromEntity).Where(item => item != null).ToList() ?? new List<FactionDto>();
    }

    public static List<Faction> ToEntitiesList(IEnumerable<FactionDto> list)
    {
        return list?.Select(ToEntity).Where(item => item != null).ToList() ?? new List<Faction>();
    }
}
