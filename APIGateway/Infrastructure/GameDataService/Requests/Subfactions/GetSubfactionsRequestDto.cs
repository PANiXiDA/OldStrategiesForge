using APIGateway.Infrastructure.GameDataService.Models.Subfactions.Params;
using GameData.Subfactions.Gen;
using System.ComponentModel.DataAnnotations;

namespace APIGateway.Infrastructure.GameDataService.Requests.Subfactions;

public class GetSubfactionsRequestDto
{
    [Required(ErrorMessage = "SearchParams is required.")]
    public SubfactionsSearchParamsDto SearchParams { get; set; } = new();

    public SubfactionsConvertParamsDto? ConvertParams { get; set; }

    public static GetSubfactionsRequest ToEntity(GetSubfactionsRequestDto obj)
    {
        return new GetSubfactionsRequest
        {
            SubfactionsSearchParams = SubfactionsSearchParamsDto.ToEntity(obj.SearchParams),
            SubfactionsConvertParams = obj.ConvertParams != null ? SubfactionsConvertParamsDto.ToEntity(obj.ConvertParams) : null
        };
    }
}
