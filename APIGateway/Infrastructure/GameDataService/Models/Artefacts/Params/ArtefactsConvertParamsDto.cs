using GameData.ConvertParams.Gen;

namespace APIGateway.Infrastructure.GameDataService.Models.Artefacts.Params;

public class ArtefactsConvertParamsDto
{
    public bool? IncludeHeroClass { get; set; }
    public bool? IncludeArtefactSet { get; set; }
    public bool? IncludeAbilities { get; set; }

    public static ArtefactsConvertParams ToEntity(ArtefactsConvertParamsDto obj)
    {
        var entity = new ArtefactsConvertParams();

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
