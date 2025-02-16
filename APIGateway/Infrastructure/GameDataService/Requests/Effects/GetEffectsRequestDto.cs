using APIGateway.Infrastructure.GameDataService.Models.Effects.Params;
using GameData.Effects.Gen;
using System.ComponentModel.DataAnnotations;

namespace APIGateway.Infrastructure.GameDataService.Requests.Effects;

public class GetEffectsRequestDto
{
    [Required(ErrorMessage = "SearchParams is required.")]
    public EffectsSearchParamsDto SearchParams { get; set; } = new();

    public EffectsConvertParamsDto? ConvertParams { get; set; }

    public static GetEffectsRequest ToEntity(GetEffectsRequestDto obj)
    {
        return new GetEffectsRequest
        {
            EffectsSearchParams = EffectsSearchParamsDto.ToEntity(obj.SearchParams),
            EffectsConvertParams = obj.ConvertParams != null ? EffectsConvertParamsDto.ToEntity(obj.ConvertParams) : null
        };
    }
}
