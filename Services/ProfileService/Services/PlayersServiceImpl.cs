using Common;
using Grpc.Core;
using Profile.Players.Gen;
using ProfileService.DAL.Interfaces;
using Common.ConvertParams.ProfileService;
using ImageService.S3Images.Gen;

namespace ProfileService.Services;

public class PlayersServiceImpl : ProfilePlayers.ProfilePlayersBase
{
    private readonly ILogger<PlayersServiceImpl> _logger;
    private readonly IPlayersDAL _playersDAL;
    private readonly S3Images.S3ImagesClient _s3ImagesClient;

    public PlayersServiceImpl(
        ILogger<PlayersServiceImpl> logger,
        IPlayersDAL playersDAL,
        S3Images.S3ImagesClient s3ImagesClient)
    {
        _logger = logger;
        _playersDAL = playersDAL;
        _s3ImagesClient = s3ImagesClient;
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
            var presignedUrlResponse = await _s3ImagesClient.GetPresignedUrlAsync(new GetPresignedUrlRequest() { S3Path = player.Avatar.S3Path });
            player.Avatar.S3Path = presignedUrlResponse.FileUrl;
        }

        return await Task.FromResult(player.GetPlayersResponseProtoFromDto());
    }
}
