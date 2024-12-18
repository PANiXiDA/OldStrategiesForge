using Common.Constants;
using Grpc.Core;
using Profile.Players.Gen;
using ProfileService.DAL.Interfaces;
using Common.ConvertParams.ProfileService;
using ImageService.S3Images.Gen;
using Google.Protobuf.WellKnownTypes;
using ProfileService.Dto;

namespace ProfileService.Services;

public class PlayersServiceImpl : ProfilePlayers.ProfilePlayersBase
{
    private readonly ILogger<PlayersServiceImpl> _logger;
    private readonly IPlayersDAL _playersDAL;
    private readonly S3Images.S3ImagesClient _s3ImagesClient;
    private readonly IAvatarsDAL _avatarsDAL;
    private readonly IFramesDAL _framesDAL;

    public PlayersServiceImpl(
        ILogger<PlayersServiceImpl> logger,
        IPlayersDAL playersDAL,
        S3Images.S3ImagesClient s3ImagesClient,
        IAvatarsDAL avatarsDAL,
        IFramesDAL framesDAL)
    {
        _logger = logger;
        _playersDAL = playersDAL;
        _s3ImagesClient = s3ImagesClient;
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
        if (player.Avatar != null)
        {
            var presignedUrlResponse = await _s3ImagesClient.GetPresignedUrlAsync(new GetPresignedUrlRequest() { S3Paths = { player.Avatar.S3Path } });
            player.Avatar.S3Path = presignedUrlResponse.FileUrls.First();
        }
        if (player.Frame != null)
        {
            var presignedUrlResponse = await _s3ImagesClient.GetPresignedUrlAsync(new GetPresignedUrlRequest() { S3Paths = { player.Frame.S3Path } });
            player.Frame.S3Path = presignedUrlResponse.FileUrls.First();
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
}
