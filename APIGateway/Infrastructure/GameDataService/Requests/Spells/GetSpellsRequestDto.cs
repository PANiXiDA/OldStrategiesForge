using APIGateway.Infrastructure.GameDataService.Models.Spells.Params;
using GameData.Spells.Gen;
using System.ComponentModel.DataAnnotations;

namespace APIGateway.Infrastructure.GameDataService.Requests.Spells;

public class GetSpellsRequestDto
{
    [Required(ErrorMessage = "SearchParams is required.")]
    public SpellsSearchParamsDto SearchParams { get; set; } = new();

    public SpellsConvertParamsDto? ConvertParams { get; set; }

    public static GetSpellsRequest ToEntity(GetSpellsRequestDto obj)
    {
        return new GetSpellsRequest
        {
            SpellsSearchParams = SpellsSearchParamsDto.ToEntity(obj.SearchParams),
            SpellsConvertParams = obj.ConvertParams != null ? SpellsConvertParamsDto.ToEntity(obj.ConvertParams) : null
        };
    }
}
