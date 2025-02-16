using GameData.Entities.Gen;
using GameData.HeroClasses.Gen;
using GameDataService.DAL.Interfaces;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace GameDataService.Services;

public class HeroClassesServiceImpl : HeroClassesService.HeroClassesServiceBase
{
    private readonly ILogger<HeroClassesServiceImpl> _logger;

    private readonly IHeroClassesDAL _heroClassesDAL;

    public HeroClassesServiceImpl(
        ILogger<HeroClassesServiceImpl> logger,
        IHeroClassesDAL heroClassesDAL)
    {
        _logger = logger;
        _heroClassesDAL = heroClassesDAL;
    }
    public override async Task<HeroClass> Get(GetHeroClassRequest request, ServerCallContext context)
    {
        var entity = await _heroClassesDAL.GetAsync(request.Id, request.HeroClassesConvertParams);

        return entity;
    }

    public override async Task<GetHeroClassesResponse> GetByFilter(GetHeroClassesRequest request, ServerCallContext context)
    {
        var entities = (await _heroClassesDAL.GetAsync(request.HeroClassesSearchParams, request.HeroClassesConvertParams)).Objects;

        return new GetHeroClassesResponse { HeroClasses = { entities } };
    }

    public override async Task<Empty> CreateOrUpdate(HeroClass entity, ServerCallContext context)
    {
        await _heroClassesDAL.AddOrUpdateAsync(entity);

        return new Empty();
    }

    public override async Task<Empty> Delete(DeleteHeroClassRequest request, ServerCallContext context)
    {
        await _heroClassesDAL.DeleteAsync(request.Id);

        return new Empty();
    }
}