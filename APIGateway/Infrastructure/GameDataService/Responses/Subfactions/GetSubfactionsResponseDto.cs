using APIGateway.Infrastructure.GameDataService.Models.Subfactions;
using GameData.Subfactions.Gen;

namespace APIGateway.Infrastructure.GameDataService.Responses.Subfactions;

public class GetSubfactionsResponseDto
{
    public List<SubfactionDto> Subfactions { get; set; } = new();

    public static GetSubfactionsResponseDto FromEntity(GetSubfactionsResponse obj)
    {
        return new GetSubfactionsResponseDto
        {
            Subfactions = SubfactionDto.FromEntitiesList(obj.Subfactions)
        };
    }
}
