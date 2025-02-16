using GameData.ConvertParams.Gen;

namespace APIGateway.Infrastructure.GameDataService.Models.Effects.Params;

public class EffectsConvertParamsDto
{
    public static EffectsConvertParams ToEntity(EffectsConvertParamsDto obj)
    {
        var entity = new EffectsConvertParams();

        return entity;
    }
}
