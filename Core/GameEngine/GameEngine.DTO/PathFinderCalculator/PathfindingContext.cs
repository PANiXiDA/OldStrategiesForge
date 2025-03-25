using System.Collections.Generic;

namespace GameEngine.DTO.PathFinderCalculator
{
    public class PathfindingContext
    {
        public int MoveRange { get; set; }
        public bool IgnoringObstacles { get; set; }
        public List<Tile> Grid { get; set; }
        public Tile Start { get; set; }
        public Tile Target { get; set; }

        public PathfindingContext(
            int moveRange,
            bool ignoringObstacles,
            List<Tile> grid,
            Tile start,
            Tile target = null)
        {
            MoveRange = moveRange;
            IgnoringObstacles = ignoringObstacles;
            Grid = grid;
            Start = start;
            Target = target;
        }
    }
}
