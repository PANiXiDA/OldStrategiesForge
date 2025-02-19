using APIGateway.Infrastructure.GameDataService.Models.Abilities.Params;
using GameData.Abilities.Gen;
using System.ComponentModel.DataAnnotations;

namespace APIGateway.Infrastructure.GameDataService.Requests.Abilities;

public class GetAbilitiesRequestDto
{
    [Required(ErrorMessage = "SearchParams is required.")]
    public AbilitiesSearchParamsDto SearchParams { get; set; } = new();

    public AbilitiesConvertParamsDto? ConvertParams { get; set; }

    public static GetAbilitiesRequest ToEntity(GetAbilitiesRequestDto obj)
    {
        return new GetAbilitiesRequest
        {
            AbilitiesSearchParams = AbilitiesSearchParamsDto.ToEntity(obj.SearchParams),
            AbilitiesConvertParams = obj.ConvertParams != null ? AbilitiesConvertParamsDto.ToEntity(obj.ConvertParams) : null
        };
    }
}
