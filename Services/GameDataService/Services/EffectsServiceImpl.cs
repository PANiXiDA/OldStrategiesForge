using GameData.Entities.Gen;
using GameData.Effects.Gen;
using GameDataService.DAL.Interfaces;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace GameDataService.Services;

public class EffectsServiceImpl : EffectsService.EffectsServiceBase
{
    private readonly ILogger<EffectsServiceImpl> _logger;

    private readonly IEffectsDAL _effectsDAL;

    public EffectsServiceImpl(
        ILogger<EffectsServiceImpl> logger,
        IEffectsDAL effectsDAL)
    {
        _logger = logger;
        _effectsDAL = effectsDAL;
    }

    public override async Task<Effect> Get(GetEffectRequest request, ServerCallContext context)
    {
        var entity = await _effectsDAL.GetAsync(request.Id, request.EffectsConvertParams);

        return entity;
    }

    public override async Task<GetEffectsResponse> GetByFilter(GetEffectsRequest request, ServerCallContext context)
    {
        var entities = (await _effectsDAL.GetAsync(request.EffectsSearchParams, request.EffectsConvertParams)).Objects;

        return new GetEffectsResponse { Effects = { entities } };
    }

    public override async Task<Empty> CreateOrUpdate(Effect entity, ServerCallContext context)
    {
        await _effectsDAL.AddOrUpdateAsync(entity);

        return new Empty();
    }

    public override async Task<Empty> Delete(DeleteEffectRequest request, ServerCallContext context)
    {
        await _effectsDAL.DeleteAsync(request.Id);

        return new Empty();
    }
}
