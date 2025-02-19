using APIGateway.Infrastructure.GameDataService.Models.Artefacts.Params;
using GameData.Artefacts.Gen;
using System.ComponentModel.DataAnnotations;

namespace APIGateway.Infrastructure.GameDataService.Requests.Artefacts;

public class GetArtefactsRequestDto
{
    [Required(ErrorMessage = "SearchParams is required.")]
    public ArtefactsSearchParamsDto SearchParams { get; set; } = new();

    public ArtefactsConvertParamsDto? ConvertParams { get; set; }

    public static GetArtefactsRequest ToEntity(GetArtefactsRequestDto obj)
    {
        return new GetArtefactsRequest
        {
            ArtefactsSearchParams = ArtefactsSearchParamsDto.ToEntity(obj.SearchParams),
            ArtefactsConvertParams = obj.ConvertParams != null ? ArtefactsConvertParamsDto.ToEntity(obj.ConvertParams) : null
        };
    }
}
