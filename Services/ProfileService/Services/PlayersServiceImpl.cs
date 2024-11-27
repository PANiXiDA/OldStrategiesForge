using Common;
using Grpc.Core;
using Profile.Players.Gen;
using ProfileService.DAL.Interfaces;
using Common.ConvertParams.ProfileService;

namespace ProfileService.Services;

public class PlayersServiceImpl : ProfilePlayers.ProfilePlayersBase
{
    private readonly ILogger<PlayersServiceImpl> _logger;
    private readonly IPlayersDAL _playersDAL;

    public PlayersServiceImpl(ILogger<PlayersServiceImpl> logger, IPlayersDAL playersDAL)
    {
        _logger = logger;
        _playersDAL = playersDAL;
    }

    public override async Task<GetPlayerResponse> Get(GetPlayerRequest request, ServerCallContext context)
    {
        var player = await _playersDAL.GetAsync(
            request.Id,
            new PlayersConvertParams()
            {
                IsIncludeChildCategories = true
            });

        if (player == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, Constants.ErrorMessages.PlayerNotFound));
        }

        return await Task.FromResult(player.GetPlayersResponseProtoFromDto());
    }
}
