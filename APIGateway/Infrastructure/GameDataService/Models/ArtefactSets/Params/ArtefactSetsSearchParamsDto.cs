using GameData.SearchParams.Gen;

namespace APIGateway.Infrastructure.GameDataService.Models.ArtefactSets.Params;

public class ArtefactSetsSearchParamsDto
{
    public static ArtefactSetsSearchParams ToEntity(ArtefactSetsSearchParamsDto obj)
    {
        var entity = new ArtefactSetsSearchParams();

        return entity;
    }
}
