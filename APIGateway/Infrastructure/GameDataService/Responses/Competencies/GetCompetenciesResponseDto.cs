using APIGateway.Infrastructure.GameDataService.Models.Competencies;
using GameData.Competencies.Gen;

namespace APIGateway.Infrastructure.GameDataService.Responses.Competencies;

public class GetCompetenciesResponseDto
{
    public List<CompetenceDto> Competencies { get; set; } = new();

    public static GetCompetenciesResponseDto FromEntity(GetCompetenciesResponse obj)
    {
        return new GetCompetenciesResponseDto
        {
            Competencies = CompetenceDto.FromEntitiesList(obj.Competencies)
        };
    }
}
