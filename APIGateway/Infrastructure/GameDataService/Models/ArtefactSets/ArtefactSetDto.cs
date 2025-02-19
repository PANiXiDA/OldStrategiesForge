using APIGateway.Infrastructure.GameDataService.Models.Artefacts;
using APIGateway.Infrastructure.GameDataService.Models.ArtefactSetBonuses;
using GameData.Entities.Gen;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace APIGateway.Infrastructure.GameDataService.Models.ArtefactSets;

public class ArtefactSetDto
{
    [Required(ErrorMessage = "Id is required.")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required.")]
    public string Description { get; set; } = string.Empty;

    [ValidateNever]
    public List<ArtefactSetBonusDto> ArtefactSetBonuses { get; set; } = new();

    [ValidateNever]
    public List<ArtefactDto> Artefacts { get; set; } = new();

    public static ArtefactSetDto FromEntity(ArtefactSet obj)
    {
        return new ArtefactSetDto
        {
            Id = obj.Id,
            Name = obj.Name,
            Description = obj.Description,
            ArtefactSetBonuses = ArtefactSetBonusDto.FromEntitiesList(obj.ArtefactSetBonuses),
            Artefacts = ArtefactDto.FromEntitiesList(obj.Artefacts)
        };
    }

    public static ArtefactSet ToEntity(ArtefactSetDto obj)
    {
        var entity = new ArtefactSet
        {
            Id = obj.Id,
            Name = obj.Name,
            Description = obj.Description
        };

        entity.ArtefactSetBonuses.AddRange(ArtefactSetBonusDto.ToEntitiesList(obj.ArtefactSetBonuses));
        entity.Artefacts.AddRange(ArtefactDto.ToEntitiesList(obj.Artefacts));

        return entity;
    }

    public static List<ArtefactSetDto> FromEntitiesList(IEnumerable<ArtefactSet> list)
    {
        return list?.Select(FromEntity).Where(item => item != null).ToList() ?? new List<ArtefactSetDto>();
    }

    public static List<ArtefactSet> ToEntitiesList(IEnumerable<ArtefactSetDto> list)
    {
        return list?.Select(ToEntity).Where(item => item != null).ToList() ?? new List<ArtefactSet>();
    }
}
