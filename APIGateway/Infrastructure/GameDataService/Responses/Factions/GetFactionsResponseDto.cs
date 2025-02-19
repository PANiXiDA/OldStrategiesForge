using APIGateway.Infrastructure.GameDataService.Models.Factions;
using GameData.Factions.Gen;

namespace APIGateway.Infrastructure.GameDataService.Responses.Factions;

public class GetFactionsResponseDto
{
    public List<FactionDto> Factions { get; set; } = new();

    public static GetFactionsResponseDto FromEntity(GetFactionsResponse obj)
    {
        return new GetFactionsResponseDto
        {
            Factions = FactionDto.FromEntitiesList(obj.Factions)
        };
    }
}
