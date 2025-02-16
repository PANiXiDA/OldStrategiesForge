using APIGateway.Infrastructure.GameDataService.Models.Effects;
using GameData.Effects.Gen;

namespace APIGateway.Infrastructure.GameDataService.Responses.Effects;

public class GetEffectsResponseDto
{
    public List<EffectDto> Effects { get; set; } = new();

    public static GetEffectsResponseDto FromEntity(GetEffectsResponse obj)
    {
        return new GetEffectsResponseDto
        {
            Effects = EffectDto.FromEntitiesList(obj.Effects)
        };
    }
}
