using APIGateway.Infrastructure.GameDataService.Models.Abilities;
using APIGateway.Infrastructure.GameDataService.Models.ArtefactSets;
using APIGateway.Infrastructure.GameDataService.Models.HeroClasses;
using GameData.Entities.Gen;
using System.ComponentModel.DataAnnotations;

namespace APIGateway.Infrastructure.GameDataService.Models.ArtefactSetBonuses;

public class ArtefactSetBonusDto
{
    [Required(ErrorMessage = "Id is required.")]
    public int Id { get; set; }

    public int? AttackBonus { get; set; }

    public int? DefenceBonus { get; set; }

    public int? HealthBonus { get; set; }

    public int? MinDamageBonus { get; set; }

    public int? MaxDamageBonus { get; set; }

    public double? InitiativeBonus { get; set; }

    public int? SpeedBonus { get; set; }

    public int? RangeBonus { get; set; }

    public int? ArrowsBonus { get; set; }

    public int? MoraleBonus { get; set; }

    public int? LuckBonus { get; set; }

    public int? HeroClassId { get; set; }

    [Required(ErrorMessage = "ArtefactSetId is required.")]
    public int ArtefactSetId { get; set; }

    public HeroClassDto? HeroClass { get; set; }

    public ArtefactSetDto? ArtefactSet { get; set; }

    public List<AbilityDto> Abilities { get; set; } = new();

    public static ArtefactSetBonusDto FromEntity(ArtefactSetBonus obj)
    {
        return new ArtefactSetBonusDto
        {
            Id = obj.Id,
            AttackBonus = obj.AttackBonus,
            DefenceBonus = obj.DefenceBonus,
            HealthBonus = obj.HealthBonus,
            MinDamageBonus = obj.MinDamageBonus,
            MaxDamageBonus = obj.MaxDamageBonus,
            InitiativeBonus = obj.InitiativeBonus,
            SpeedBonus = obj.SpeedBonus,
            RangeBonus = obj.RangeBonus,
            ArrowsBonus = obj.ArrowsBonus,
            MoraleBonus = obj.MoraleBonus,
            LuckBonus = obj.LuckBonus,
            HeroClassId = obj.HeroClassId,
            ArtefactSetId = obj.ArtefactSetId,
            HeroClass = obj.HeroClass != null ? HeroClassDto.FromEntity(obj.HeroClass) : null,
            ArtefactSet = obj.ArtefactSet != null ? ArtefactSetDto.FromEntity(obj.ArtefactSet) : null,
            Abilities = AbilityDto.FromEntitiesList(obj.Abilities)
        };
    }

    public static ArtefactSetBonus ToEntity(ArtefactSetBonusDto obj)
    {
        var entity = new ArtefactSetBonus
        {
            Id = obj.Id,
            AttackBonus = obj.AttackBonus,
            DefenceBonus = obj.DefenceBonus,
            HealthBonus = obj.HealthBonus,
            MinDamageBonus = obj.MinDamageBonus,
            MaxDamageBonus = obj.MaxDamageBonus,
            InitiativeBonus = obj.InitiativeBonus,
            SpeedBonus = obj.SpeedBonus,
            RangeBonus = obj.RangeBonus,
            ArrowsBonus = obj.ArrowsBonus,
            MoraleBonus = obj.MoraleBonus,
            LuckBonus = obj.LuckBonus,
            HeroClassId = obj.HeroClassId,
            ArtefactSetId = obj.ArtefactSetId,
            HeroClass = obj.HeroClass != null ? HeroClassDto.ToEntity(obj.HeroClass) : null,
            ArtefactSet = obj.ArtefactSet != null ? ArtefactSetDto.ToEntity(obj.ArtefactSet) : null
        };

        entity.Abilities.AddRange(AbilityDto.ToEntitiesList(obj.Abilities));

        return entity;
    }

    public static List<ArtefactSetBonusDto> FromEntitiesList(IEnumerable<ArtefactSetBonus> list)
    {
        return list?.Select(FromEntity).Where(item => item != null).ToList() ?? new List<ArtefactSetBonusDto>();
    }

    public static List<ArtefactSetBonus> ToEntitiesList(IEnumerable<ArtefactSetBonusDto> list)
    {
        return list?.Select(ToEntity).Where(item => item != null).ToList() ?? new List<ArtefactSetBonus>();
    }
}
