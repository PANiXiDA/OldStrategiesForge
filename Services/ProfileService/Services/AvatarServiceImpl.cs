using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Profile.Avatar.Gen;
using ProfileService.DAL.Interfaces;
using ProfileService.Dto;
using Tools.AWS3.Interfaces;

namespace ProfileService.Services;

public class AvatarServiceImpl : ProfileAvatars.ProfileAvatarsBase
{
    private readonly ILogger<AuthServiceImpl> _logger;
    private readonly IAvatarsDAL _avatarsDAL;
    private readonly IS3Client _client;

    public AvatarServiceImpl(
        ILogger<AuthServiceImpl> logger,
        IAvatarsDAL avatarsDAL,
        IS3Client client)
    {
        _logger = logger;
        _avatarsDAL = avatarsDAL;
        _client = client;
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

    public override async Task<GetPresignedUrlResponse> GetPresignedUrl(GetPresignedUrlRequest request, ServerCallContext context)
    {
        var url = await _client.GeneratePresignedUrl(request.S3Path, TimeSpan.FromHours(1));

        return new GetPresignedUrlResponse() { FileUrl = url };
    }
}
