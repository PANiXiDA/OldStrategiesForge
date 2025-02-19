using APIGateway.Infrastructure.GameDataService.Models.ArtefactSetBonuses.Params;
using GameData.ArtefactSetBonuses.Gen;
using System.ComponentModel.DataAnnotations;

namespace APIGateway.Infrastructure.GameDataService.Requests.ArtefactSetBonuses;

public class GetArtefactSetBonusesRequestDto
{
    [Required(ErrorMessage = "SearchParams is required.")]
    public ArtefactSetBonusesSearchParamsDto SearchParams { get; set; } = new();

    public ArtefactSetBonusesConvertParamsDto? ConvertParams { get; set; }

    public static GetArtefactSetBonusesRequest ToEntity(GetArtefactSetBonusesRequestDto obj)
    {
        return new GetArtefactSetBonusesRequest
        {
            ArtefactSetBonusesSearchParams = ArtefactSetBonusesSearchParamsDto.ToEntity(obj.SearchParams),
            ArtefactSetBonusesConvertParams = obj.ConvertParams != null ? ArtefactSetBonusesConvertParamsDto.ToEntity(obj.ConvertParams) : null
        };
    }
}
