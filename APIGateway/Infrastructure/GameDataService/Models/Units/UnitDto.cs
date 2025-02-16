using APIGateway.Infrastructure.GameDataService.Models.Abilities;
using APIGateway.Infrastructure.GameDataService.Models.Factions;
using GameData.Entities.Gen;
using System.ComponentModel.DataAnnotations;

namespace APIGateway.Infrastructure.GameDataService.Models.Units;

public class UnitDto
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

    [Required(ErrorMessage = "Health is required.")]
    public int Health { get; set; }

    [Required(ErrorMessage = "MinDamage is required.")]
    public int MinDamage { get; set; }

    [Required(ErrorMessage = "MaxDamage is required.")]
    public int MaxDamage { get; set; }

    [Required(ErrorMessage = "Initiative is required.")]
    public double Initiative { get; set; }

    [Required(ErrorMessage = "Speed is required.")]
    public int Speed { get; set; }

    [Required(ErrorMessage = "Range is required.")]
    public int? Range { get; set; }

    [Required(ErrorMessage = "Arrows is required.")]
    public int? Arrows { get; set; }

    [Required(ErrorMessage = "Morale is required.")]
    public int Morale { get; set; }

    [Required(ErrorMessage = "Luck is required.")]
    public int Luck { get; set; }

    public int? FactionId { get; set; }

    public int? BaseUnitId { get; set; }

    public FactionDto? Faction { get; set; }

    public UnitDto? BaseUnit { get; set; }

    public List<UnitDto> Upgrades { get; set; } = new();

    public List<AbilityDto> Abilities { get; set; } = new();

    public static UnitDto FromEntity(Unit obj)
    {
        return new UnitDto
        {
            Id = obj.Id,
            Name = obj.Name,
            Description = obj.Description,
            Attack = obj.Attack,
            Defence = obj.Defence,
            Health = obj.Health,
            MinDamage = obj.MinDamage,
            MaxDamage = obj.MaxDamage,
            Initiative = obj.Initiative,
            Speed = obj.Speed,
            Range = obj.Range,
            Arrows = obj.Arrows,
            Morale = obj.Morale,
            Luck = obj.Luck,
            FactionId = obj.FactionId,
            BaseUnitId = obj.BaseUnitId,
            Faction = obj.Faction != null ? FactionDto.FromEntity(obj.Faction) : null,
            BaseUnit = obj.BaseUnit != null ? FromEntity(obj.BaseUnit) : null,
            Upgrades = FromEntitiesList(obj.Upgrades),
            Abilities = AbilityDto.FromEntitiesList(obj.Abilities)
        };
    }

    public static Unit ToEntity(UnitDto obj)
    {
        var entity = new Unit
        {
            Id = obj.Id,
            Name = obj.Name,
            Description = obj.Description,
            Attack = obj.Attack,
            Defence = obj.Defence,
            Health = obj.Health,
            MinDamage = obj.MinDamage,
            MaxDamage = obj.MaxDamage,
            Initiative = obj.Initiative,
            Speed = obj.Speed,
            Range = obj.Range,
            Arrows = obj.Arrows,
            Morale = obj.Morale,
            Luck = obj.Luck,
            FactionId = obj.FactionId,
            BaseUnitId = obj.BaseUnitId,
            Faction = obj.Faction != null ? FactionDto.ToEntity(obj.Faction) : null,
            BaseUnit = obj.BaseUnit != null ? ToEntity(obj.BaseUnit) : null
        };

        entity.Upgrades.AddRange(ToEntitiesList(obj.Upgrades));
        entity.Abilities.AddRange(AbilityDto.ToEntitiesList(obj.Abilities));

        return entity;
    }

    public static List<UnitDto> FromEntitiesList(IEnumerable<Unit> list)
    {
        return list?.Select(FromEntity).Where(item => item != null).ToList() ?? new List<UnitDto>();
    }

    public static List<Unit> ToEntitiesList(IEnumerable<UnitDto> list)
    {
        return list?.Select(ToEntity).Where(item => item != null).ToList() ?? new List<Unit>();
    }
}
