using GameData.SearchParams.Gen;

namespace APIGateway.Infrastructure.GameDataService.Models.Factions.Params;

public class FactionsSearchParamsDto
{
    public static FactionsSearchParams ToEntity(FactionsSearchParamsDto obj)
    {
        var entity = new FactionsSearchParams();

        return entity;
    }
}
