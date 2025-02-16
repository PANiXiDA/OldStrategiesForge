using GameData.Entities.Gen;
using GameData.Factions.Gen;
using GameDataService.DAL.Interfaces;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace GameDataService.Services;

public class FactionsServiceImpl : FactionsService.FactionsServiceBase
{
    private readonly ILogger<FactionsServiceImpl> _logger;

    private readonly IFactionsDAL _factionsDAL;

    public FactionsServiceImpl(
        ILogger<FactionsServiceImpl> logger,
        IFactionsDAL factionsDAL)
    {
        _logger = logger;
        _factionsDAL = factionsDAL;
    }

    public override async Task<Faction> Get(GetFactionRequest request, ServerCallContext context)
    {
        var entity = await _factionsDAL.GetAsync(request.Id, request.FactionsConvertParams);

        return entity;
    }

    public override async Task<GetFactionsResponse> GetByFilter(GetFactionsRequest request, ServerCallContext context)
    {
        var entities = (await _factionsDAL.GetAsync(request.FactionsSearchParams, request.FactionsConvertParams)).Objects;

        return new GetFactionsResponse { Factions = { entities } };
    }

    public override async Task<Empty> CreateOrUpdate(Faction entity, ServerCallContext context)
    {
        await _factionsDAL.AddOrUpdateAsync(entity);

        return new Empty();
    }

    public override async Task<Empty> Delete(DeleteFactionRequest request, ServerCallContext context)
    {
        await _factionsDAL.DeleteAsync(request.Id);

        return new Empty();
    }
}
