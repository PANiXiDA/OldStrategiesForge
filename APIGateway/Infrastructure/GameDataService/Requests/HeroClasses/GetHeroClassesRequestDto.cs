using APIGateway.Infrastructure.GameDataService.Models.HeroClasses.Params;
using GameData.HeroClasses.Gen;
using System.ComponentModel.DataAnnotations;

namespace APIGateway.Infrastructure.GameDataService.Requests.HeroClasses;

public class GetHeroClassesRequestDto
{
    [Required(ErrorMessage = "SearchParams is required.")]
    public HeroClassesSearchParamsDto SearchParams { get; set; } = new();

    public HeroClassesConvertParamsDto? ConvertParams { get; set; }

    public static GetHeroClassesRequest ToEntity(GetHeroClassesRequestDto obj)
    {
        return new GetHeroClassesRequest
        {
            HeroClassesSearchParams = HeroClassesSearchParamsDto.ToEntity(obj.SearchParams),
            HeroClassesConvertParams = obj.ConvertParams != null ? HeroClassesConvertParamsDto.ToEntity(obj.ConvertParams) : null
        };
    }
}
