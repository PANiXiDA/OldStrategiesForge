using GameData.SearchParams.Gen;

namespace APIGateway.Infrastructure.GameDataService.Models.Subfactions.Params;

public class SubfactionsSearchParamsDto
{
    public int? FactionId { get; set; }

    public static SubfactionsSearchParams ToEntity(SubfactionsSearchParamsDto obj)
    {
        var entity = new SubfactionsSearchParams();

        if (obj.FactionId.HasValue)
        {
            entity.FactionId = obj.FactionId.Value;
        }

        return entity;
    }
}
