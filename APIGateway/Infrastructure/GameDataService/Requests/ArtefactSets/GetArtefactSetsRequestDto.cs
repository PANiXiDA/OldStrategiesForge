using APIGateway.Infrastructure.GameDataService.Models.ArtefactSets.Params;
using GameData.ArtefactSets.Gen;
using System.ComponentModel.DataAnnotations;

namespace APIGateway.Infrastructure.GameDataService.Requests.ArtefactSets;

public class GetArtefactSetsRequestDto
{
    [Required(ErrorMessage = "SearchParams is required.")]
    public ArtefactSetsSearchParamsDto SearchParams { get; set; } = new();

    public ArtefactSetsConvertParamsDto? ConvertParams { get; set; }

    public static GetArtefactSetsRequest ToEntity(GetArtefactSetsRequestDto obj)
    {
        return new GetArtefactSetsRequest
        {
            ArtefactSetsSearchParams = ArtefactSetsSearchParamsDto.ToEntity(obj.SearchParams),
            ArtefactSetsConvertParams = obj.ConvertParams != null ? ArtefactSetsConvertParamsDto.ToEntity(obj.ConvertParams) : null
        };
    }
}
