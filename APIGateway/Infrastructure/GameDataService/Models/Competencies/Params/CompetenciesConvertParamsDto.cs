using GameData.ConvertParams.Gen;

namespace APIGateway.Infrastructure.GameDataService.Models.Competencies.Params;

public class CompetenciesConvertParamsDto
{
    public bool? IncludeSubfaction { get; set; }
    public bool? IncludeSkills { get; set; }

    public static CompetenciesConvertParams ToEntity(CompetenciesConvertParamsDto obj)
    {
        var entity = new CompetenciesConvertParams();

        if (obj.IncludeSubfaction.HasValue)
        {
            entity.IncludeSubfaction = obj.IncludeSubfaction.Value;
        }
        if (obj.IncludeSkills.HasValue)
        {
            entity.IncludeSkills = obj.IncludeSkills.Value;
        }

        return entity;
    }
}
