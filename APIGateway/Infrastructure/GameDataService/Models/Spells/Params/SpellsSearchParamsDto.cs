using GameData.SearchParams.Gen;

namespace APIGateway.Infrastructure.GameDataService.Models.Spells.Params;

public class SpellsSearchParamsDto
{
    public int? RequiredSkillId { get; set; }

    public static SpellsSearchParams ToEntity(SpellsSearchParamsDto obj)
    {
        var entity = new SpellsSearchParams();

        if (obj.RequiredSkillId.HasValue)
        {
            entity.RequiredSkillId = obj.RequiredSkillId.Value;
        }

        return entity;
    }
}
