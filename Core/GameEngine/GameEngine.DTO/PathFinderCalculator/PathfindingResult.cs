using System.Collections.Generic;

namespace GameEngine.DTO.PathFinderCalculator
{
    public class PathfindingResult
    {
        public Dictionary<Tile, double> Distances { get; }
        public Dictionary<Tile, Tile> CameFrom { get; }
        public bool TargetReached { get; set; }

        public PathfindingResult()
        {
            Distances = new Dictionary<Tile, double>();
            CameFrom = new Dictionary<Tile, Tile>();
            TargetReached = false;
        }
    }
}
