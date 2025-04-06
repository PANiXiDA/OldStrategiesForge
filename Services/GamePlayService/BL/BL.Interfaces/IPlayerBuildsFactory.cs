using GameEngine.Domains;

namespace GamePlayService.BL.BL.Interfaces;

public interface IPlayerBuildsFactory
{
    Task<(Hero, List<Unit>)> GetGameEntities(string buildId);
}
