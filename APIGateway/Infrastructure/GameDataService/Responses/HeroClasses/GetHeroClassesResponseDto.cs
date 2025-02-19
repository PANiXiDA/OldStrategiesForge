using APIGateway.Infrastructure.GameDataService.Models.HeroClasses;
using GameData.HeroClasses.Gen;

namespace APIGateway.Infrastructure.GameDataService.Responses.HeroClasses;

public class GetHeroClassesResponseDto
{
    public List<HeroClassDto> HeroClasses { get; set; } = new();

    public static GetHeroClassesResponseDto FromEntity(GetHeroClassesResponse obj)
    {
        return new GetHeroClassesResponseDto
        {
            HeroClasses = HeroClassDto.FromEntitiesList(obj.HeroClasses)
        };
    }
}
