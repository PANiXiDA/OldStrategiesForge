using GameData.ConvertParams.Gen;

namespace APIGateway.Infrastructure.GameDataService.Models.Abilities.Params;

public class AbilitiesConvertParamsDto
{
    public bool? IncludeEffects { get; set; }

    public static AbilitiesConvertParams ToEntity(AbilitiesConvertParamsDto obj)
    {
        var entity = new AbilitiesConvertParams();

        if (obj.IncludeEffects.HasValue)
        {
            entity.IncludeEffects = obj.IncludeEffects.Value;
        }

        return entity;
    }
}
