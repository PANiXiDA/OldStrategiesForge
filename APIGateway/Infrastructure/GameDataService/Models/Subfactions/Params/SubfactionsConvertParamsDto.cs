using GameData.ConvertParams.Gen;

namespace APIGateway.Infrastructure.GameDataService.Models.Subfactions.Params;

public class SubfactionsConvertParamsDto
{
    public bool? IncludeFaction { get; set; }
    public bool? IncludeSkills { get; set; }
    public bool? IncludeAbilities { get; set; }

    public static SubfactionsConvertParams ToEntity(SubfactionsConvertParamsDto obj)
    {
        var entity = new SubfactionsConvertParams();

        if (obj.IncludeFaction.HasValue)
        {
            entity.IncludeFaction = obj.IncludeFaction.Value;
        }
        if (obj.IncludeSkills.HasValue)
        {
            entity.IncludeSkills = obj.IncludeSkills.Value;
        }
        if (obj.IncludeAbilities.HasValue)
        {
            entity.IncludeAbilities = obj.IncludeAbilities.Value;
        }

        return entity;
    }
}
