using GameData.Enums.Gen;
using GameData.SearchParams.Gen;

namespace APIGateway.Infrastructure.GameDataService.Models.Effects.Params;

public class EffectsSearchParamsDto
{
    public EffectType? EffectType { get; set; }

    public static EffectsSearchParams ToEntity(EffectsSearchParamsDto obj)
    {
        var entity = new EffectsSearchParams();

        if (obj.EffectType.HasValue)
        {
            entity.EffectType = obj.EffectType.Value;
        }

        return entity;
    }
}
