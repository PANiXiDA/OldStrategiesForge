using GameData.Abilities.Gen;
using GameData.Entities.Gen;
using GameDataService.DAL.Interfaces;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace GameDataService.Services;

public class AbilitiesServiceImpl : AbilitiesService.AbilitiesServiceBase
{
    private readonly ILogger<AbilitiesServiceImpl> _logger;

    private readonly IAbilitiesDAL _abilitiesDAL;

    public AbilitiesServiceImpl(
        ILogger<AbilitiesServiceImpl> logger,
        IAbilitiesDAL abilitiesDAL)
    {
        _logger = logger;
        _abilitiesDAL = abilitiesDAL;
    }

    public override async Task<Ability> Get(GetAbilityRequest request, ServerCallContext context)
    {
        var entity = await _abilitiesDAL.GetAsync(request.Id, request.AbilitiesConvertParams);

        return entity;
    }

    public override async Task<GetAbilitiesResponse> GetByFilter(GetAbilitiesRequest request, ServerCallContext context)
    {
        var entities = (await _abilitiesDAL.GetAsync(request.AbilitiesSearchParams, request.AbilitiesConvertParams)).Objects;

        return new GetAbilitiesResponse { Abilities = { entities } };
    }

    public override async Task<Empty> CreateOrUpdate(Ability entity, ServerCallContext context)
    {
        await _abilitiesDAL.AddOrUpdateAsync(entity);

        return new Empty();
    }

    public override async Task<Empty> Delete(DeleteAbilityRequest request, ServerCallContext context)
    {
        await _abilitiesDAL.DeleteAsync(request.Id);

        return new Empty();
    }
}