using APIGateway.Infrastructure.GameDataService.Models.Skills;
using APIGateway.Infrastructure.GameDataService.Models.Subfactions;
using GameData.Entities.Gen;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace APIGateway.Infrastructure.GameDataService.Models.Competencies;

public class CompetenceDto
{
    [Required(ErrorMessage = "Id is required.")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required.")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "SubfactionId is required.")]
    public int SubfactionId { get; set; }

    [ValidateNever]
    public SubfactionDto? Subfaction { get; set; }

    [ValidateNever]
    public List<SkillDto> Skills { get; set; } = new();

    public static CompetenceDto FromEntity(Competence obj)
    {
        return new CompetenceDto
        {
            Id = obj.Id,
            Name = obj.Name,
            Description = obj.Description,
            SubfactionId = obj.SubfactionId,
            Subfaction = obj.Subfaction != null ? SubfactionDto.FromEntity(obj.Subfaction) : null,
            Skills = SkillDto.FromEntitiesList(obj.Skills)
        };
    }

    public static Competence ToEntity(CompetenceDto obj)
    {
        var entity = new Competence
        {
            Id = obj.Id,
            Name = obj.Name,
            Description = obj.Description,
            SubfactionId = obj.SubfactionId,
            Subfaction = obj.Subfaction != null ? SubfactionDto.ToEntity(obj.Subfaction) : null
        };

        entity.Skills.AddRange(SkillDto.ToEntitiesList(obj.Skills));

        return entity;
    }

    public static List<CompetenceDto> FromEntitiesList(IEnumerable<Competence> list)
    {
        return list?.Select(FromEntity).Where(item => item != null).ToList() ?? new List<CompetenceDto>();
    }

    public static List<Competence> ToEntitiesList(IEnumerable<CompetenceDto> list)
    {
        return list?.Select(ToEntity).Where(item => item != null).ToList() ?? new List<Competence>();
    }
}
