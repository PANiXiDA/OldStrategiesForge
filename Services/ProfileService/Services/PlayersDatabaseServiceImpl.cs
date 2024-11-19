using Players.Database.Gen;
using ProfileService.DAL.Interfaces;

namespace ProfileService.Services;

public class PlayersDatabaseServiceImpl : PlayersDatabase.PlayersDatabaseBase
{
    private readonly ILogger<PlayersDatabaseServiceImpl> _logger;
    private readonly IPlayersDAL _playersDAL;

    public PlayersDatabaseServiceImpl(ILogger<PlayersDatabaseServiceImpl> logger, IPlayersDAL playersDAL)
    {
        _logger = logger;
        _playersDAL = playersDAL;
    }
}
