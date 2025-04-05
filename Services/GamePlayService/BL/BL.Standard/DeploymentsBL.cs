using GameEngine.DTO.PathFinderCalculator;
using GameEngine.Interfaces;
using GamePlayService.BL.BL.Interfaces;
using GamePlayService.Extensions.Helpers;
using GamePlayService.Infrastructure.Models;

namespace GamePlayService.BL.BL.Standard;

public class DeploymentsBL : IDeploymentsBL
{
    private ILogger<DeploymentsBL> _logger;

    private readonly IGridGenerator _gridGenerator;

    public DeploymentsBL(
        ILogger<DeploymentsBL> logger,
        IGridGenerator gridGenerator)
    {
        _logger = logger;
        _gridGenerator = gridGenerator;
    }

    public bool ValidateDeployment(GameSession gameSession, int playerId, List<Tile> deployment)
    {
        var player = gameSession.Players.FirstOrDefault(player => player.Id == playerId);
        if (player == null)
        {
            _logger.LogError($"В текущей сессии нет игрока с id: {playerId}");
            return false;
        }

        var deploymentUnitIds = deployment.Where(tile => tile.OccupiedUnitId.HasValue).Select(tile => tile.OccupiedUnitId!.Value).OrderBy(id => id).ToList();
        var playerUnitIds = player.Units.Select(unit => unit.Id).OrderBy(id => id).ToList();
        if (!deploymentUnitIds.SequenceEqual(playerUnitIds))
        {
            return false;
        }

        var minX = gameSession.RoundState.Grid.Min(tile => tile.X);
        var maxX = gameSession.RoundState.Grid.Max(tile => tile.X);
        var minY = gameSession.RoundState.Grid.Min(tile => tile.Y);
        var maxY = gameSession.RoundState.Grid.Max(tile => tile.Y);

        var coordinates = _gridGenerator.GetDeploymentCoordinates(minX, maxX, minY, maxY, player.Side, player.ColumnsToDeployment);

        var deploymentCoordinates = deployment.Select(tile => (tile.X, tile.Y)).ToList();

        if (!deploymentCoordinates.All(coordinate => coordinates.Contains(coordinate)))
        {
            return false;
        }

        if (deploymentCoordinates.Distinct().Count() != deploymentCoordinates.Count)
        {
            return false;
        }

        if (!deploymentCoordinates.All(coordinate =>
        {
            var gridTile = gameSession.RoundState.Grid
                .FirstOrDefault(tile => tile.X == coordinate.Item1 && tile.Y == coordinate.Item2);
            return gridTile != null && (gridTile.IsWalkable || gridTile.OccupiedUnitId.HasValue);
        }))
        {
            return false;
        }

        return true;
    }

    public void ApplyDeployment(List<Tile> grid, List<Tile> deployment)
    {
        foreach (var deployedTile in deployment)
        {
            var gridTile = grid.FirstOrDefault(tile => tile.X == deployedTile.X && tile.Y == deployedTile.Y);
            if (gridTile != null)
            {
                gridTile.OccupiedUnitId = deployedTile.OccupiedUnitId;
                gridTile.IsWalkable = deployedTile.IsWalkable;
            }
        }
    }

}
