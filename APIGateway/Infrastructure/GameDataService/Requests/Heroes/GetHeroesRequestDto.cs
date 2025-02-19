using APIGateway.Infrastructure.GameDataService.Models.Heroes.Params;
using GameData.Heroes.Gen;
using System.ComponentModel.DataAnnotations;

namespace APIGateway.Infrastructure.GameDataService.Requests.Heroes;

public class GetHeroesRequestDto
{
    [Required(ErrorMessage = "SearchParams is required.")]
    public HeroesSearchParamsDto SearchParams { get; set; } = new();

    public HeroesConvertParamsDto? ConvertParams { get; set; }

    public static GetHeroesRequest ToEntity(GetHeroesRequestDto obj)
    {
        return new GetHeroesRequest
        {
            HeroesSearchParams = HeroesSearchParamsDto.ToEntity(obj.SearchParams),
            HeroesConvertParams = obj.ConvertParams != null ? HeroesConvertParamsDto.ToEntity(obj.ConvertParams) : null
        };
    }
}
