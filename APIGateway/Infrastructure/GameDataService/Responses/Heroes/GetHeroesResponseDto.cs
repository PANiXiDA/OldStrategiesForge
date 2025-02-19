using APIGateway.Infrastructure.GameDataService.Models.Heroes;
using GameData.Heroes.Gen;

namespace APIGateway.Infrastructure.GameDataService.Responses.Heroes;

public class GetHeroesResponseDto
{
    public List<HeroDto> Heroes { get; set; } = new();

    public static GetHeroesResponseDto FromEntity(GetHeroesResponse obj)
    {
        return new GetHeroesResponseDto
        {
            Heroes = HeroDto.FromEntitiesList(obj.Heroes)
        };
    }
}
