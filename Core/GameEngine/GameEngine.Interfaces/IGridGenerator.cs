using GameEngine.Domains;
using GameEngine.DTO.Enums;
using GameEngine.DTO.PathFinderCalculator;
using System.Collections.Generic;

namespace GameEngine.Interfaces
{
    public interface IGridGenerator
    {
        List<Tile> GenerateGrid(GameType gameType);
        List<Tile> BaseDeploymentGrid(List<Tile> grid, List<Unit> units, PlayerSide side, int columns);
        List<(int x, int y)> GetDeploymentCoordinates(int minX, int maxX, int minY, int maxY, PlayerSide side, int columns);
    }
}
