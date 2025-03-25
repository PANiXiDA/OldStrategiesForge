using GameEngine.DTO.PathFinderCalculator;
using System.Collections.Generic;

namespace GameEngine.Interfaces
{
    public interface IPathFinderCalculator
    {
        List<Tile> GetPath(PathfindingContext context);
        bool IsReachable(PathfindingContext context);
        List<Tile> GetReachableTiles(PathfindingContext context);
    }
}
