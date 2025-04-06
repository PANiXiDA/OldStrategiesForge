using GameEngine.DTO.PathFinderCalculator;

namespace GamePlayService.Infrastructure.Requests;

public class DeploymentMessage
{
    public List<Tile> Deployment { get; set; } = new List<Tile>();
}
