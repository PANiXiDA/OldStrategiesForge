using Grpc.Core;
using Players.Database.Gen;
using ProfileDatabaseService.DAL.Interfaces;

namespace ProfileDatabaseService.Services;

public class PlayersDatabaseServiceImpl : PlayersDatabase.PlayersDatabaseBase
{
    private readonly ILogger<PlayersDatabaseServiceImpl> _logger;
    //private readonly IPlayersDAL _playersDAL;

    public PlayersDatabaseServiceImpl(ILogger<PlayersDatabaseServiceImpl> logger)
    {
        _logger = logger;
        //_playersDAL = playersDAL;
    }

    public override Task<CreatePlayerResponse> Create(CreatePlayerRequest request, ServerCallContext context)
    {
        var response = new CreatePlayerResponse
        {
            Message = "Player created successfully"
        };

        return Task.FromResult(response);
    }
}
