using GameData.ConvertParams.Gen;

namespace APIGateway.Infrastructure.GameDataService.Models.ArtefactSets.Params;

public class ArtefactSetsConvertParamsDto
{
    public bool? IncludeArtefactSetBonus { get; set; }
    public bool? IncludeArtefacts { get; set; }

    public static ArtefactSetsConvertParams ToEntity(ArtefactSetsConvertParamsDto obj)
    {
        var entity = new ArtefactSetsConvertParams();

        if (obj.IncludeArtefactSetBonus.HasValue)
        {
            entity.IncludeArtefactSetBonus = obj.IncludeArtefactSetBonus.Value;
        }
        if (obj.IncludeArtefacts.HasValue)
        {
            entity.IncludeArtefacts = obj.IncludeArtefacts.Value;
        }

        return entity;
    }
}
