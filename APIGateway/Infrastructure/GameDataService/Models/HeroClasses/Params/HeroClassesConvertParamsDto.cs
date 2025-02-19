using GameData.ConvertParams.Gen;

namespace APIGateway.Infrastructure.GameDataService.Models.HeroClasses.Params;

public class HeroClassesConvertParamsDto
{
    public bool? IncludeAbilities { get; set; }

    public static HeroClassesConvertParams ToEntity(HeroClassesConvertParamsDto obj)
    {
        var entity = new HeroClassesConvertParams();

        if (obj.IncludeAbilities.HasValue)
        {
            entity.IncludeAbilities = obj.IncludeAbilities.Value;
        }

        return entity;
    }
}
