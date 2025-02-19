using APIGateway.Infrastructure.GameDataService.Models.Spells;
using GameData.Spells.Gen;

namespace APIGateway.Infrastructure.GameDataService.Responses.Spells;

public class GetSpellsResponseDto
{
    public List<SpellDto> Spells { get; set; } = new();

    public static GetSpellsResponseDto FromEntity(GetSpellsResponse obj)
    {
        return new GetSpellsResponseDto
        {
            Spells = SpellDto.FromEntitiesList(obj.Spells)
        };
    }
}
