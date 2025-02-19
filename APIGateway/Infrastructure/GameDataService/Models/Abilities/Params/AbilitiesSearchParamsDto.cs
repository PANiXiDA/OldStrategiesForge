using GameData.SearchParams.Gen;

namespace APIGateway.Infrastructure.GameDataService.Models.Abilities.Params;

public class AbilitiesSearchParamsDto
{
    public static AbilitiesSearchParams ToEntity(AbilitiesSearchParamsDto obj)
    {
        var entity = new AbilitiesSearchParams();

        return entity;
    }
}
