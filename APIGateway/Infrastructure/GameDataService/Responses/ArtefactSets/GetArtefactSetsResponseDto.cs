using APIGateway.Infrastructure.GameDataService.Models.ArtefactSets;
using GameData.ArtefactSets.Gen;

namespace APIGateway.Infrastructure.GameDataService.Responses.ArtefactSets;

public class GetArtefactSetsResponseDto
{
    public List<ArtefactSetDto> ArtefactSets { get; set; } = new();

    public static GetArtefactSetsResponseDto FromEntity(GetArtefactSetsResponse obj)
    {
        return new GetArtefactSetsResponseDto
        {
            ArtefactSets = ArtefactSetDto.FromEntitiesList(obj.ArtefactSets)
        };
    }
}
