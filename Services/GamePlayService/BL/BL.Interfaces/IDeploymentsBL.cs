using GameEngine.DTO.PathFinderCalculator;
using GamePlayService.Infrastructure.Models;

namespace GamePlayService.BL.BL.Interfaces;

public interface IDeploymentsBL
{
    bool ValidateDeployment(GameSession gameSession, int playerId, List<Tile> deployment);
    void ApplyDeployment(List<Tile> grid, List<Tile> deployment);
}
