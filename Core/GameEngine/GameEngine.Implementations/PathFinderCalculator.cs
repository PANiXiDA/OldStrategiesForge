using GameEngine.DTO.PathFinderCalculator;
using GameEngine.DTO.Core;
using System.Collections.Generic;
using System;
using System.Linq;
using GameEngine.Interfaces;

namespace GameEngine.Implementations
{
    public class PathFinderCalculator : IPathFinderCalculator
    {
        public List<Tile> GetPath(PathfindingContext context)
        {
            if (context.Target == null)
            {
                throw new ArgumentException("Target must be provided for IsReachable.");
            }

            var searchResult = PerformSearch(context);
            if (!searchResult.TargetReached)
            {
                return new List<Tile>();
            }

            return ReconstructPath(searchResult.CameFrom, context.Start, context.Target);
        }

        public bool IsReachable(PathfindingContext context)
        {
            if (context.Target == null)
            {
                throw new ArgumentException("Target must be provided for IsReachable.");
            }

            var searchResult = PerformSearch(context);

            return searchResult.TargetReached;
        }

        public List<Tile> GetReachableTiles(PathfindingContext context)
        {
            var modifiedContext = new PathfindingContext(
                context.MoveRange,
                context.IgnoringObstacles,
                context.Grid,
                context.Start,
                target: null);

            var searchResult = PerformSearch(modifiedContext);

            return searchResult.Distances
                .Where(kvp => kvp.Value <= context.MoveRange && kvp.Value < double.MaxValue)
                .Select(kvp => kvp.Key)
                .ToList();
        }

        private PathfindingResult PerformSearch(PathfindingContext context)
        {
            var result = new PathfindingResult();
            var gridDict = context.Grid.ToDictionary(tile => (tile.X, tile.Y));
            var tilesToProcess = new PriorityQueue<Tile, double>();

            foreach (var tile in context.Grid)
            {
                result.Distances[tile] = double.MaxValue;
            }
            result.Distances[context.Start] = 0;
            tilesToProcess.Enqueue(context.Start, 0);

            while (tilesToProcess.Count > 0)
            {
                var current = tilesToProcess.Dequeue();

                if (context.Target != null && current == context.Target)
                {
                    result.TargetReached = true;
                    break;
                }

                foreach (var movementVector in Direction.MovementVectors)
                {
                    int neighborX = current.X + movementVector.X;
                    int neighborY = current.Y + movementVector.Y;

                    if (!gridDict.TryGetValue((neighborX, neighborY), out var
                        neighbor))
                    {
                        continue;
                    }

                    if (!context.IgnoringObstacles && !neighbor.IsWalkable)
                    {
                        continue;
                    }

                    double newCost = result.Distances[current] + movementVector.Cost;
                    if (newCost > context.MoveRange)
                    {
                        continue;
                    }

                    if (newCost < result.Distances[neighbor])
                    {
                        result.Distances[neighbor] = newCost;
                        if (context.Target != null)
                        {
                            result.CameFrom[neighbor] = current;
                        }
                        tilesToProcess.Enqueue(neighbor, newCost);
                    }
                }
            }

            return result;
        }

        private List<Tile> ReconstructPath(Dictionary<Tile, Tile> cameFrom, Tile start, Tile target)
        {
            var path = new List<Tile>();
            var current = target;

            while (cameFrom.ContainsKey(current))
            {
                path.Add(current);
                current = cameFrom[current];
            }

            if (current == start)
            {
                path.Add(start);
            }

            path.Reverse();

            return path;
        }
    }
}
