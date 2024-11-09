using Players.Database.Gen;
using ProfileDatabaseService.DAL.Interfaces;

namespace ProfileDatabaseService.Services;

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
