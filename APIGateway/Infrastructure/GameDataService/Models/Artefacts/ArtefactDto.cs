using APIGateway.Infrastructure.GameDataService.Models.Abilities;
using APIGateway.Infrastructure.GameDataService.Models.ArtefactSets;
using APIGateway.Infrastructure.GameDataService.Models.HeroClasses;
using GameData.Entities.Gen;
using GameData.Enums.Gen;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace APIGateway.Infrastructure.GameDataService.Models.Artefacts;

public class ArtefactDto
{
    [Required(ErrorMessage = "Id is required.")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required.")]
    public string Description { get; set; } = string.Empty;

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

    [Required(ErrorMessage = "ArtefactSlot is required.")]
    public ArtefactSlot ArtefactSlot { get; set; }

    public int? HeroClassId { get; set; }

    public int? ArtefactSetId { get; set; }

    [ValidateNever]
    public HeroClassDto? HeroClass { get; set; }

    [ValidateNever]
    public ArtefactSetDto? ArtefactSet { get; set; }

    [ValidateNever]
    public List<AbilityDto> Abilities { get; set; } = new();

    public static ArtefactDto FromEntity(Artefact obj)
    {
        return new ArtefactDto
        {
            Id = obj.Id,
            Name = obj.Name,
            Description = obj.Description,
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
            ArtefactSlot = obj.ArtefactSlot,
            HeroClassId = obj.HeroClassId,
            ArtefactSetId = obj.ArtefactSetId,
            HeroClass = obj.HeroClass != null ? HeroClassDto.FromEntity(obj.HeroClass) : null,
            ArtefactSet = obj.ArtefactSet != null ? ArtefactSetDto.FromEntity(obj.ArtefactSet) : null,
            Abilities = AbilityDto.FromEntitiesList(obj.Abilities)
        };
    }

    public static Artefact ToEntity(ArtefactDto obj)
    {
        var entity = new Artefact
        {
            Id = obj.Id,
            Name = obj.Name,
            Description = obj.Description,
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
            ArtefactSlot = obj.ArtefactSlot,
            HeroClassId = obj.HeroClassId,
            ArtefactSetId = obj.ArtefactSetId,
            HeroClass = obj.HeroClass != null ? HeroClassDto.ToEntity(obj.HeroClass) : null,
            ArtefactSet = obj.ArtefactSet != null ? ArtefactSetDto.ToEntity(obj.ArtefactSet) : null
        };

        entity.Abilities.AddRange(AbilityDto.ToEntitiesList(obj.Abilities));

        return entity;
    }

    public static List<ArtefactDto> FromEntitiesList(IEnumerable<Artefact> list)
    {
        return list?.Select(FromEntity).Where(item => item != null).ToList() ?? new List<ArtefactDto>();
    }

    public static List<Artefact> ToEntitiesList(IEnumerable<ArtefactDto> list)
    {
        return list?.Select(ToEntity).Where(item => item != null).ToList() ?? new List<Artefact>();
    }
}
