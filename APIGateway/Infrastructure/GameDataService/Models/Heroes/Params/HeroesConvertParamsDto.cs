using GameData.ConvertParams.Gen;

namespace APIGateway.Infrastructure.GameDataService.Models.Heroes.Params;

public class HeroesConvertParamsDto
{
    public bool? IncludeSubfaction { get; set; }
    public bool? IncludeHeroClass { get; set; }
    public bool? IncludeAbilities { get; set; }
    public bool? IncludeArtefacts { get; set; }

    public static HeroesConvertParams ToEntity(HeroesConvertParamsDto obj)
    {
        var entity = new HeroesConvertParams();

        if (obj.IncludeSubfaction.HasValue)
        {
            entity.IncludeSubfaction = obj.IncludeSubfaction.Value;
        }
        if (obj.IncludeHeroClass.HasValue)
        {
            entity.IncludeHeroClass = obj.IncludeHeroClass.Value;
        }
        if (obj.IncludeAbilities.HasValue)
        {
            entity.IncludeAbilities = obj.IncludeAbilities.Value;
        }
        if (obj.IncludeArtefacts.HasValue)
        {
            entity.IncludeArtefacts = obj.IncludeArtefacts.Value;
        }

        return entity;
    }
}
