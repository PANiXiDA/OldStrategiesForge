using APIGateway.Infrastructure.GameDataService.Models.Factions.Params;
using GameData.Factions.Gen;
using System.ComponentModel.DataAnnotations;

namespace APIGateway.Infrastructure.GameDataService.Requests.Factions;

public class GetFactionsRequestDto
{
    [Required(ErrorMessage = "SearchParams is required.")]
    public FactionsSearchParamsDto SearchParams { get; set; } = new();

    public FactionsConvertParamsDto? ConvertParams { get; set; }

    public static GetFactionsRequest ToEntity(GetFactionsRequestDto obj)
    {
        return new GetFactionsRequest
        {
            FactionsSearchParams = FactionsSearchParamsDto.ToEntity(obj.SearchParams),
            FactionsConvertParams = obj.ConvertParams != null ? FactionsConvertParamsDto.ToEntity(obj.ConvertParams) : null
        };
    }
}
