using GameData.SearchParams.Gen;

namespace APIGateway.Infrastructure.GameDataService.Models.Units.Params;

public class UnitsSearchParamsDto
{
    public static UnitsSearchParams ToEntity(UnitsSearchParamsDto obj)
    {
        var entity = new UnitsSearchParams();

        return entity;
    }
}
