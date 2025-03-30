using GameEngine.Domains;
using GameEngine.DTO.PathFinderCalculator;

namespace GamePlayService.Infrastructure.Responses;

public class DeploymentStartMessage
{
    public List<Tile> Grid { get; set; }
    public List<Unit> Units { get; set; }

    public DeploymentStartMessage(
        List<Tile> grid,
        List<Unit> units)
    {
        Grid = grid;
        Units = units;
    }
}
