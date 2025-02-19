using GameData.ConvertParams.Gen;

namespace APIGateway.Infrastructure.GameDataService.Models.Factions.Params;

public class FactionsConvertParamsDto
{
    public bool? IncludeAbilities { get; set; }

    public static FactionsConvertParams ToEntity(FactionsConvertParamsDto obj)
    {
        var entity = new FactionsConvertParams();

        if (obj.IncludeAbilities.HasValue)
        {
            entity.IncludeAbilities = obj.IncludeAbilities.Value;
        }

        return entity;
    }
}
