using GameData.SearchParams.Gen;

namespace APIGateway.Infrastructure.GameDataService.Models.ArtefactSetBonuses.Params;

public class ArtefactSetBonusesSearchParamsDto
{
    public static ArtefactSetBonusesSearchParams ToEntity(ArtefactSetBonusesSearchParamsDto obj)
    {
        var entity = new ArtefactSetBonusesSearchParams();

        return entity;
    }
}
