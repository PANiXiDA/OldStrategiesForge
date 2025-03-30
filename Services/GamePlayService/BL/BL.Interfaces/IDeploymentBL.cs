using GameEngine.DTO.PathFinderCalculator;
using GamePlayService.Infrastructure.Models;

namespace GamePlayService.BL.BL.Interfaces;

public interface IDeploymentBL
{
    bool ValidateDeployment(GameSession gameSession, string authToken, List<Tile> deployment);
    void ApplyDeployment(List<Tile> grid, List<Tile> deployment);
}
