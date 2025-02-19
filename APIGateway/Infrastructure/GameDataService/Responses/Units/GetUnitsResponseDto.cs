using APIGateway.Infrastructure.GameDataService.Models.Units;
using GameData.Units.Gen;

namespace APIGateway.Infrastructure.GameDataService.Responses.Units;

public class GetUnitsResponseDto
{
    public List<UnitDto> Units { get; set; } = new();

    public static GetUnitsResponseDto FromEntity(GetUnitsResponse obj)
    {
        return new GetUnitsResponseDto
        {
            Units = UnitDto.FromEntitiesList(obj.Units)
        };
    }
}
