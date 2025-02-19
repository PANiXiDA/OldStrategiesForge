using APIGateway.Infrastructure.GameDataService.Models.Skills.Params;
using GameData.Skills.Gen;
using System.ComponentModel.DataAnnotations;

namespace APIGateway.Infrastructure.GameDataService.Requests.Skills;

public class GetSkillsRequestDto
{
    [Required(ErrorMessage = "SearchParams is required.")]
    public SkillsSearchParamsDto SearchParams { get; set; } = new();

    public SkillsConvertParamsDto? ConvertParams { get; set; }

    public static GetSkillsRequest ToEntity(GetSkillsRequestDto obj)
    {
        return new GetSkillsRequest
        {
            SkillsSearchParams = SkillsSearchParamsDto.ToEntity(obj.SearchParams),
            SkillsConvertParams = obj.ConvertParams != null ? SkillsConvertParamsDto.ToEntity(obj.ConvertParams) : null
        };
    }
}
