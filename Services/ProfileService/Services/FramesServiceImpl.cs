using Common.SearchParams.ProfileService;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using ImageService.S3Images.Gen;
using Profile.Frames.Gen;
using ProfileService.DAL.Interfaces;
using ProfileService.Dto;
using Tools.AWS3.Interfaces;

namespace ProfileService.Services;

public class FramesServiceImpl : ProfileFrames.ProfileFramesBase
{
    private readonly ILogger<FramesServiceImpl> _logger;
    private readonly IFramesDAL _framesDAL;
    private readonly IS3Client _client;
    private readonly S3Images.S3ImagesClient _s3ImagesClient;

    public FramesServiceImpl(
        ILogger<FramesServiceImpl> logger,
        IFramesDAL framesDAL,
        IS3Client client,
        S3Images.S3ImagesClient s3ImagesClient)
    {
        _logger = logger;
        _framesDAL = framesDAL;
        _client = client;
        _s3ImagesClient = s3ImagesClient;
    }

    public override async Task<Empty> Create(CreateFrameRequest request, ServerCallContext context)
    {
        await _framesDAL.AddOrUpdateAsync(new FramesDto(
            id: 0,
            s3Path: request.S3Path,
            name: request.Name,
            description: request.Description,
            necessaryMmr: request.NecessaryMmr,
            necessaryGames: request.NecessaryGames,
            necessaryWins: request.NecessaryWins,
            available: request.Available));

        return new Empty();
    }

    public override async Task<GetFrameResponse> Get(GetFrameRequest request, ServerCallContext context)
    {
        var avatar = await _framesDAL.GetAsync(request.Id);

        return avatar.AvatarsProtoGetFromDto();
    }

    public override async Task<GetFramesResponse> GetAvailable(Empty request, ServerCallContext context)
    {
        var avatars = (await _framesDAL.GetAsync(new FramesSearchParams()
        {
            IsAvailable = true
        })).Objects.ToList();

        var s3Paths = avatars.Select(a => a.S3Path).ToList();
        var presignedUrlResponse = await _s3ImagesClient.GetPresignedUrlAsync(new GetPresignedUrlRequest
        {
            S3Paths = { s3Paths }
        });

        var responseAvatars = new GetFramesResponse();
        for (int i = 0; i < avatars.Count; i++)
        {
            avatars[i].S3Path = presignedUrlResponse.FileUrls[i];
            responseAvatars.Avatars.Add(avatars[i].AvatarsProtoGetFromDto());
        }

        return responseAvatars;
    }

    public override async Task<Empty> Update(UpdateFrameRequest request, ServerCallContext context)
    {
        await _framesDAL.AddOrUpdateAsync(new FramesDto(
            id: request.Id,
            s3Path: request.S3Path,
            name: request.Name,
            description: request.Description,
            necessaryMmr: request.NecessaryMmr,
            necessaryGames: request.NecessaryGames,
            necessaryWins: request.NecessaryWins,
            available: request.Available));

        return new Empty();
    }

    public override async Task<Empty> Delete(DeleteFrameRequest request, ServerCallContext context)
    {
        await _framesDAL.DeleteAsync(request.Id);

        return new Empty();
    }
}
