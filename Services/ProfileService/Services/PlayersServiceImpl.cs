using Common.Constants;
using Grpc.Core;
using Profile.Players.Gen;
using ProfileService.DAL.Interfaces;
using Common.ConvertParams.ProfileService;
using Google.Protobuf.WellKnownTypes;
using ProfileService.Dto;
using Common.SearchParams.ProfileService;

namespace ProfileService.Services;

public class PlayersServiceImpl : ProfilePlayers.ProfilePlayersBase
{
    private readonly ILogger<PlayersServiceImpl> _logger;
    private readonly IPlayersDAL _playersDAL;
    private readonly IAvatarsDAL _avatarsDAL;
    private readonly IFramesDAL _framesDAL;

    public PlayersServiceImpl(
        ILogger<PlayersServiceImpl> logger,
        IPlayersDAL playersDAL,
        IAvatarsDAL avatarsDAL,
        IFramesDAL framesDAL)
    {
        _logger = logger;
        _playersDAL = playersDAL;
        _avatarsDAL = avatarsDAL;
        _framesDAL = framesDAL;
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
            throw new RpcException(new Status(StatusCode.NotFound, ErrorMessages.PlayerNotFound));
        }

        return await Task.FromResult(player.GetPlayersResponseProtoFromDto());
    }

    public override async Task<Empty> UpdateAvatar(UpdateAvatarRequest request, ServerCallContext context)
    {
        var player = await _playersDAL.GetAsync(request.PlayerId);
        if (player == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, ErrorMessages.PlayerNotFound));
        }

        var avatar = await _avatarsDAL.GetAsync(request.AvatarId);
        if (avatar == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, ErrorMessages.AvatarNotFound));
        }

        ValidateItemRequirements(avatar, player);

        player.AvatarId = request.AvatarId;
        await _playersDAL.AddOrUpdateAsync(player);

        return new Empty();
    }

    public override async Task<Empty> UpdateFrame(UpdateFrameRequest request, ServerCallContext context)
    {
        var player = await _playersDAL.GetAsync(request.PlayerId);
        if (player == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, ErrorMessages.PlayerNotFound));
        }

        var frame = await _framesDAL.GetAsync(request.FrameId);
        if (frame == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, ErrorMessages.AvatarNotFound));
        }

        ValidateItemRequirements(frame, player);

        player.FrameId = request.FrameId;
        await _playersDAL.AddOrUpdateAsync(player);

        return new Empty();
    }

    public override async Task<UpdatePlayersStatisticAfterGameResponse> UpdatePlayersStatisticAfterGame(UpdatePlayersStatisticAfterGameRequest request, ServerCallContext context)
    {
        var result = new List<UpdatePlayersStatisticAfterGameResult>();

        var players = (await _playersDAL.GetAsync(new PlayersSearchParams()
        {
            Ids = request.PlayerIds.ToList()
        })).Objects.ToList();

        var winner = players.FirstOrDefault(player => player.Id == request.WinnerId);
        var loserMaxMmr = players.Where(player => player.Id != request.WinnerId).Max(player => player.Mmr);

        foreach (var player in players)
        {
            if (winner == null)
            {
                continue;
            }

            int mmrChanges = 0;
            if (player.Id != winner.Id)
            {
                mmrChanges = GeMmrChanges(
                    playerMmr: player.Mmr,
                    opponentMmr: winner.Mmr,
                    isWin: false);

                player.Loses += 1;
            }
            else
            {
                mmrChanges = GeMmrChanges(
                    playerMmr: winner.Mmr,
                    opponentMmr: loserMaxMmr,
                    isWin: true);

                player.Wins += 1;
            }

            player.Games += 1;
            player.Mmr += mmrChanges;

            var updatePlayersStatisticAfterGameResult = new UpdatePlayersStatisticAfterGameResult()
            {
                MmrChanges = mmrChanges,
                Nickname = player.Nickname,
            };
            result.Add(updatePlayersStatisticAfterGameResult);
        }

        await _playersDAL.AddOrUpdateAsync(players);

        var response = new UpdatePlayersStatisticAfterGameResponse();
        response.UpdatePlayersStatisticResult.AddRange(result);
        return response;
    }

    private void ValidateItemRequirements(dynamic item, PlayersDto player)
    {
        if (!item.Available)
        {
            throw new RpcException(new Status(StatusCode.PermissionDenied, ErrorMessages.AvatarNotAvailable));
        }

        if (item.NecessaryMmr > player.Mmr)
        {
            throw new RpcException(new Status(StatusCode.FailedPrecondition, $"{ErrorMessages.NotEnoughMmr} {item.NecessaryMmr}"));
        }
        if (item.NecessaryGames > player.Games)

        {
            throw new RpcException(new Status(StatusCode.FailedPrecondition, $"{ErrorMessages.NotEnoughGames} {item.NecessaryGames}"));
        }

        if (item.NecessaryWins > player.Wins)
        {
            throw new RpcException(new Status(StatusCode.FailedPrecondition, $"{ErrorMessages.NotEnoughWins} {item.NecessaryWins}"));
        }
    }

    private int GeMmrChanges(int playerMmr, int opponentMmr, bool isWin)
    {
        int difference = Math.Abs(playerMmr - opponentMmr);

        int change = difference / 25;

        if (change < 1) change = 1;
        if (change > 16) change = 16;

        return isWin ? change : -change;
    }
}
