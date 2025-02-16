using GameData.Entities.Gen;
using GameData.Subfactions.Gen;
using GameDataService.DAL.Interfaces;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace GameDataService.Services;

public class SubfactionsServiceImpl : SubfactionsService.SubfactionsServiceBase
{
    private readonly ILogger<SubfactionsServiceImpl> _logger;

    private readonly ISubfactionsDAL _subfactionsDAL;

    public SubfactionsServiceImpl(
        ILogger<SubfactionsServiceImpl> logger,
        ISubfactionsDAL subfactionsDAL)
    {
        _logger = logger;
        _subfactionsDAL = subfactionsDAL;
    }

    public override async Task<Subfaction> Get(GetSubfactionRequest request, ServerCallContext context)
    {
        var entity = await _subfactionsDAL.GetAsync(request.Id, request.SubfactionsConvertParams);

        return entity;
    }

    public override async Task<GetSubfactionsResponse> GetByFilter(GetSubfactionsRequest request, ServerCallContext context)
    {
        var entities = (await _subfactionsDAL.GetAsync(request.SubfactionsSearchParams, request.SubfactionsConvertParams)).Objects;

        return new GetSubfactionsResponse { Subfactions = { entities } };
    }

    public override async Task<Empty> CreateOrUpdate(Subfaction entity, ServerCallContext context)
    {
        await _subfactionsDAL.AddOrUpdateAsync(entity);

        return new Empty();
    }

    public override async Task<Empty> Delete(DeleteSubfactionRequest request, ServerCallContext context)
    {
        await _subfactionsDAL.DeleteAsync(request.Id);

        return new Empty();
    }
}
