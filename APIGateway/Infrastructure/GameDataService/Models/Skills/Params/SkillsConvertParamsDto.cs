using GameData.ConvertParams.Gen;

namespace APIGateway.Infrastructure.GameDataService.Models.Skills.Params;

public class SkillsConvertParamsDto
{
    public bool? IncludeCompetence { get; set; }
    public bool? IncludeSubfaction { get; set; }
    public bool? IncludeAbility { get; set; }

    public static SkillsConvertParams ToEntity(SkillsConvertParamsDto obj)
    {
        var entity = new SkillsConvertParams();

        if (obj.IncludeCompetence.HasValue)
        {
            entity.IncludeCompetence = obj.IncludeCompetence.Value;
        }
        if (obj.IncludeSubfaction.HasValue)
        {
            entity.IncludeSubfaction = obj.IncludeSubfaction.Value;
        }
        if (obj.IncludeAbility.HasValue)
        {
            entity.IncludeAbility = obj.IncludeAbility.Value;
        }

        return entity;
    }
}
