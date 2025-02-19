using APIGateway.Infrastructure.GameDataService.Models.Abilities;
using GameData.Abilities.Gen;

namespace APIGateway.Infrastructure.GameDataService.Responses.Abilities;

public class GetAbilitiesResponseDto
{
    public List<AbilityDto> Abilities { get; set; } = new();

    public static GetAbilitiesResponseDto FromEntity(GetAbilitiesResponse obj)
    {
        return new GetAbilitiesResponseDto
        {
            Abilities = AbilityDto.FromEntitiesList(obj.Abilities)
        };
    }
}
