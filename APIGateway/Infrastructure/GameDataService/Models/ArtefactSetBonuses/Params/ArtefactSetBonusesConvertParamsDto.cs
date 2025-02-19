using GameData.ConvertParams.Gen;

namespace APIGateway.Infrastructure.GameDataService.Models.ArtefactSetBonuses.Params;

public class ArtefactSetBonusesConvertParamsDto
{
    public bool? IncludeHeroClass { get; set; }
    public bool? IncludeArtefactSet { get; set; }
    public bool? IncludeAbilities { get; set; }

    public static ArtefactSetBonusesConvertParams ToEntity(ArtefactSetBonusesConvertParamsDto obj)
    {
        var entity = new ArtefactSetBonusesConvertParams();

        if (obj.IncludeHeroClass.HasValue)
        {
            entity.IncludeHeroClass = obj.IncludeHeroClass.Value;
        }
        if (obj.IncludeArtefactSet.HasValue)
        {
            entity.IncludeArtefactSet = obj.IncludeArtefactSet.Value;
        }
        if (obj.IncludeAbilities.HasValue)
        {
            entity.IncludeAbilities = obj.IncludeAbilities.Value;
        }

        return entity;
    }
}
