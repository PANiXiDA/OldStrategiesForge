using GameData.SearchParams.Gen;

namespace APIGateway.Infrastructure.GameDataService.Models.Heroes.Params;

public class HeroesSearchParamsDto
{
    public static HeroesSearchParams ToEntity(HeroesSearchParamsDto obj)
    {
        var entity = new HeroesSearchParams();

        return entity;
    }
}
