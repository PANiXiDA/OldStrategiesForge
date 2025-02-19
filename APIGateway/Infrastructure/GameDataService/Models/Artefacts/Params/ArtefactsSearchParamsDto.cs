using GameData.SearchParams.Gen;

namespace APIGateway.Infrastructure.GameDataService.Models.Artefacts.Params;

public class ArtefactsSearchParamsDto
{
    public static ArtefactsSearchParams ToEntity(ArtefactsSearchParamsDto obj)
    {
        var entity = new ArtefactsSearchParams();

        return entity;
    }
}
