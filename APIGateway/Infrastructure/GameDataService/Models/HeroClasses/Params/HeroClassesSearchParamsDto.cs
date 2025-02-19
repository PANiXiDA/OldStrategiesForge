using GameData.SearchParams.Gen;

namespace APIGateway.Infrastructure.GameDataService.Models.HeroClasses.Params;

public class HeroClassesSearchParamsDto
{
    public static HeroClassesSearchParams ToEntity(HeroClassesSearchParamsDto obj)
    {
        var entity = new HeroClassesSearchParams();

        return entity;
    }
}
