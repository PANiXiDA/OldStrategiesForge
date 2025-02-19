using GameData.ConvertParams.Gen;

namespace APIGateway.Infrastructure.GameDataService.Models.Spells.Params;

public class SpellsConvertParamsDto
{
    public bool? IncludeRequiredSkill { get; set; }
    public bool? IncludeAbilities { get; set; }

    public static SpellsConvertParams ToEntity(SpellsConvertParamsDto obj)
    {
        var entity = new SpellsConvertParams();

        if (obj.IncludeRequiredSkill.HasValue)
        {
            entity.IncludeRequiredSkill = obj.IncludeRequiredSkill.Value;
        }
        if (obj.IncludeAbilities.HasValue)
        {
            entity.IncludeAbilities = obj.IncludeAbilities.Value;
        }

        return entity;
    }
}
