using APIGateway.Infrastructure.GameDataService.Models.Competencies.Params;
using GameData.Competencies.Gen;
using System.ComponentModel.DataAnnotations;

namespace APIGateway.Infrastructure.GameDataService.Requests.Competencies;

public class GetCompetenciesRequestDto
{
    [Required(ErrorMessage = "SearchParams is required.")]
    public CompetenciesSearchParamsDto SearchParams { get; set; } = new();

    public CompetenciesConvertParamsDto? ConvertParams { get; set; }

    public static GetCompetenciesRequest ToEntity(GetCompetenciesRequestDto obj)
    {
        return new GetCompetenciesRequest
        {
            CompetenciesSearchParams = CompetenciesSearchParamsDto.ToEntity(obj.SearchParams),
            CompetenciesConvertParams = obj.ConvertParams != null ? CompetenciesConvertParamsDto.ToEntity(obj.ConvertParams) : null
        };
    }
}
