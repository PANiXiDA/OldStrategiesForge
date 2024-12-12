using Common;
using Grpc.Core;
using Profile.Players.Gen;
using ProfileService.DAL.Interfaces;
using Common.ConvertParams.ProfileService;
using ImageService.S3Images.Gen;
using Google.Protobuf.WellKnownTypes;

namespace ProfileService.Services;

public class PlayersServiceImpl : ProfilePlayers.ProfilePlayersBase
{
    private readonly ILogger<PlayersServiceImpl> _logger;
    private readonly IPlayersDAL _playersDAL;
    private readonly S3Images.S3ImagesClient _s3ImagesClient;
    private readonly IAvatarsDAL _avatarsDAL;

    public PlayersServiceImpl(
        ILogger<PlayersServiceImpl> logger,
        IPlayersDAL playersDAL,
        S3Images.S3ImagesClient s3ImagesClient,
        IAvatarsDAL avatarsDAL)
    {
        _logger = logger;
        _playersDAL = playersDAL;
        _s3ImagesClient = s3ImagesClient;
        _avatarsDAL = avatarsDAL;
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

        if (player.Avatar != null)
        {
            var presignedUrlResponse = await _s3ImagesClient.GetPresignedUrlAsync(new GetPresignedUrlRequest() { S3Paths = { player.Avatar.S3Path } });
            player.Avatar.S3Path = presignedUrlResponse.FileUrls.First();
        }

        return await Task.FromResult(player.GetPlayersResponseProtoFromDto());
    }

    public override async Task<Empty> UpdateAvatar(UpdateAvatarRequest request, ServerCallContext context)
    {
        var playerTask = _playersDAL.GetAsync(request.PlayerId);
        var avatarTask = _avatarsDAL.GetAsync(request.AvatarId);

        await Task.WhenAll(playerTask, avatarTask);

        var player = await playerTask;
        if (player == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, Constants.ErrorMessages.PlayerNotFound));
        }

        var avatar = await avatarTask;
        if (avatar == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, Constants.ErrorMessages.AvatarNotFound));
        }

        if (avatar.Available == false)
        {
            throw new RpcException(new Status(StatusCode.PermissionDenied, Constants.ErrorMessages.AvatarNotAvailable));
        }

        if (avatar.NecessaryMmr > player.Mmr)
        {
            throw new RpcException(new Status(StatusCode.FailedPrecondition, $"{Constants.ErrorMessages.NotEnoughMmr} {avatar.NecessaryMmr}"));
        }
        if (avatar.NecessaryGames > player.Games)
        {
            throw new RpcException(new Status(StatusCode.FailedPrecondition, $"{Constants.ErrorMessages.NotEnoughGames} {avatar.NecessaryGames}"));
        }
        if (avatar.NecessaryWins > player.Wins)
        {
            throw new RpcException(new Status(StatusCode.FailedPrecondition, $"{Constants.ErrorMessages.NotEnoughWins} {avatar.NecessaryWins}"));
        }

        player.AvatarId = request.AvatarId;
        await _playersDAL.AddOrUpdateAsync(player);

        return new Empty();
    }
}
