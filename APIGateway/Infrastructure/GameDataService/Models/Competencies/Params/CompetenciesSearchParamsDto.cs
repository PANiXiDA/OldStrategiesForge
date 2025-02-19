using GameData.SearchParams.Gen;

namespace APIGateway.Infrastructure.GameDataService.Models.Competencies.Params;

public class CompetenciesSearchParamsDto
{
    public int? SubfactionId { get; set; }

    public static CompetenciesSearchParams ToEntity(CompetenciesSearchParamsDto obj)
    {
        var entity = new CompetenciesSearchParams();

        if (obj.SubfactionId.HasValue)
        {
            entity.SubfactionId = obj.SubfactionId.Value;
        }

        return entity;
    }
}
