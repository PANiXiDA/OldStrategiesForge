using GameData.Entities.Gen;
using GameData.Heroes.Gen;
using GameDataService.DAL.Interfaces;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace GameDataService.Services;

public class HeroesServiceImpl : HeroesService.HeroesServiceBase
{
    private readonly ILogger<HeroesServiceImpl> _logger;

    private readonly IHeroesDAL _heroesDAL;

    public HeroesServiceImpl(
        ILogger<HeroesServiceImpl> logger,
        IHeroesDAL heroesDAL)
    {
        _logger = logger;
        _heroesDAL = heroesDAL;
    }

    public override async Task<Hero> Get(GetHeroRequest request, ServerCallContext context)
    {
        var entity = await _heroesDAL.GetAsync(request.Id, request.HeroesConvertParams);

        return entity;
    }

    public override async Task<GetHeroesResponse> GetByFilter(GetHeroesRequest request, ServerCallContext context)
    {
        var entities = (await _heroesDAL.GetAsync(request.HeroesSearchParams, request.HeroesConvertParams)).Objects;

        return new GetHeroesResponse { Heroes = { entities } };
    }

    public override async Task<Empty> CreateOrUpdate(Hero entity, ServerCallContext context)
    {
        await _heroesDAL.AddOrUpdateAsync(entity);

        return new Empty();
    }

    public override async Task<Empty> Delete(DeleteHeroRequest request, ServerCallContext context)
    {
        await _heroesDAL.DeleteAsync(request.Id);

        return new Empty();
    }
}
