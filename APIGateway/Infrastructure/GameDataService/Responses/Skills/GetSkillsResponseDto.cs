using APIGateway.Infrastructure.GameDataService.Models.Skills;
using GameData.Skills.Gen;

namespace APIGateway.Infrastructure.GameDataService.Responses.Skills;

public class GetSkillsResponseDto
{
    public List<SkillDto> Skills { get; set; } = new();

    public static GetSkillsResponseDto FromEntity(GetSkillsResponse obj)
    {
        return new GetSkillsResponseDto
        {
            Skills = SkillDto.FromEntitiesList(obj.Skills)
        };
    }
}
