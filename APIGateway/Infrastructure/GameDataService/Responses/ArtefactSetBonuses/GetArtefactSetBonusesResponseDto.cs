using APIGateway.Infrastructure.GameDataService.Models.ArtefactSetBonuses;
using GameData.ArtefactSetBonuses.Gen;

namespace APIGateway.Infrastructure.GameDataService.Responses.ArtefactSetBonuses;

public class GetArtefactSetBonusesResponseDto
{
    public List<ArtefactSetBonusDto> ArtefactSetBonuses { get; set; } = new();

    public static GetArtefactSetBonusesResponseDto FromEntity(GetArtefactSetBonusesResponse obj)
    {
        return new GetArtefactSetBonusesResponseDto
        {
            ArtefactSetBonuses = ArtefactSetBonusDto.FromEntitiesList(obj.ArtefactSetBonuses)
        };
    }
}
