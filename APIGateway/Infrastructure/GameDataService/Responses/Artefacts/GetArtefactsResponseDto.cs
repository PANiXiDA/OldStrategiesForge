using APIGateway.Infrastructure.GameDataService.Models.Artefacts;
using GameData.Artefacts.Gen;

namespace APIGateway.Infrastructure.GameDataService.Responses.Artefacts;

public class GetArtefactsResponseDto
{
    public List<ArtefactDto> tArtefacts { get; set; } = new();

    public static GetArtefactsResponseDto FromEntity(GetArtefactsResponse obj)
    {
        return new GetArtefactsResponseDto
        {
            tArtefacts = ArtefactDto.FromEntitiesList(obj.Artefacts)
        };
    }
}
