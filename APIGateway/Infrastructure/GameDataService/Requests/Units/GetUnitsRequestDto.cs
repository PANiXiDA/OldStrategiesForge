using APIGateway.Infrastructure.GameDataService.Models.Units.Params;
using GameData.Units.Gen;
using System.ComponentModel.DataAnnotations;

namespace APIGateway.Infrastructure.GameDataService.Requests.Units;

public class GetUnitsRequestDto
{
    [Required(ErrorMessage = "SearchParams is required.")]
    public UnitsSearchParamsDto SearchParams { get; set; } = new();

    public UnitsConvertParamsDto? ConvertParams { get; set; }

    public static GetUnitsRequest ToEntity(GetUnitsRequestDto obj)
    {
        return new GetUnitsRequest
        {
            UnitsSearchParams = UnitsSearchParamsDto.ToEntity(obj.SearchParams),
            UnitsConvertParams = obj.ConvertParams != null ? UnitsConvertParamsDto.ToEntity(obj.ConvertParams) : null
        };
    }
}
