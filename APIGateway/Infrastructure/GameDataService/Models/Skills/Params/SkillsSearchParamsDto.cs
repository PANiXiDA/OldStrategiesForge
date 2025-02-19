using GameData.SearchParams.Gen;

namespace APIGateway.Infrastructure.GameDataService.Models.Skills.Params;

public class SkillsSearchParamsDto
{
    public int? SubfactionId { get; set; }

    public static SkillsSearchParams ToEntity(SkillsSearchParamsDto obj)
    {
        var entity = new SkillsSearchParams();

        if (obj.SubfactionId.HasValue)
        {
            entity.SubfactionId = obj.SubfactionId.Value;
        }

        return entity;
    }
}
