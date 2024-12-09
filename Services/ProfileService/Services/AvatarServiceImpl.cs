using Common.SearchParams.ProfileService;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Profile.Avatar.Gen;
using ProfileService.DAL.Interfaces;
using ProfileService.Dto;
using Tools.AWS3.Interfaces;
using ImageService.S3Images.Gen;

namespace ProfileService.Services;

public class AvatarServiceImpl : ProfileAvatars.ProfileAvatarsBase
{
    private readonly ILogger<AuthServiceImpl> _logger;
    private readonly IAvatarsDAL _avatarsDAL;
    private readonly IS3Client _client;
    private readonly S3Images.S3ImagesClient _s3ImagesClient;

    public AvatarServiceImpl(
        ILogger<AuthServiceImpl> logger,
        IAvatarsDAL avatarsDAL,
        IS3Client client,
        S3Images.S3ImagesClient s3ImagesClient)
    {
        _logger = logger;
        _avatarsDAL = avatarsDAL;
        _client = client;
        _s3ImagesClient = s3ImagesClient;
    }

    public override async Task<Empty> Create(CreateAvatarRequest request, ServerCallContext context)
    {
        await _avatarsDAL.AddOrUpdateAsync(new AvatarsDto(
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

    public override async Task<GetAvatarResponse> Get(GetAvatarRequest request, ServerCallContext context)
    {
        var avatar = await _avatarsDAL.GetAsync(request.Id);

        return avatar.AvatarsProtoGetFromDto();
    }

    public override async Task<GetAvatarsResponse> GetAvailable(Empty request, ServerCallContext context)
    {
        var avatars = (await _avatarsDAL.GetAsync(new AvatarsSearchParams()
        {
            IsAvailable = true
        })).Objects.ToList();

        var s3Paths = avatars.Select(a => a.S3Path).ToList();
        var presignedUrlResponse = await _s3ImagesClient.GetPresignedUrlAsync(new GetPresignedUrlRequest
        {
            S3Paths = { s3Paths }
        });

        var responseAvatars = new GetAvatarsResponse();
        for (int i = 0; i < avatars.Count; i++)
        {
            avatars[i].S3Path = presignedUrlResponse.FileUrls[i];
            responseAvatars.Avatars.Add(avatars[i].AvatarsProtoGetFromDto());
        }

        return responseAvatars;
    }

    public override async Task<Empty> Update(UpdateAvatarRequest request, ServerCallContext context)
    {
        await _avatarsDAL.AddOrUpdateAsync(new AvatarsDto(
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

    public override async Task<Empty> Delete(DeleteAvatarRequest request, ServerCallContext context)
    {
        await _avatarsDAL.DeleteAsync(request.Id);

        return new Empty();
    }
}
