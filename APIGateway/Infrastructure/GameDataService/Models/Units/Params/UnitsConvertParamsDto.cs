using GameData.ConvertParams.Gen;

namespace APIGateway.Infrastructure.GameDataService.Models.Units.Params;

public class UnitsConvertParamsDto
{
    public bool? IncludeFaction { get; set; }
    public bool? IncludeBaseUnit { get; set; }
    public bool? IncludeUpgrades { get; set; }
    public bool? IncludeAbilities { get; set; }

    public static UnitsConvertParams ToEntity(UnitsConvertParamsDto obj)
    {
        var entity = new UnitsConvertParams();

        if (obj.IncludeFaction.HasValue)
        {
            entity.IncludeFaction = obj.IncludeFaction.Value;
        }
        if (obj.IncludeBaseUnit.HasValue)
        {
            entity.IncludeBaseUnit = obj.IncludeBaseUnit.Value;
        }
        if (obj.IncludeUpgrades.HasValue)
        {
            entity.IncludeUpgrades = obj.IncludeUpgrades.Value;
        }
        if (obj.IncludeAbilities.HasValue)
        {
            entity.IncludeAbilities = obj.IncludeAbilities.Value;
        }

        return entity;
    }
}
