using GameData.Entities.Gen;
using GameData.Units.Gen;
using GameDataService.DAL.Interfaces;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace GameDataService.Services;

public class UnitsServiceImpl : UnitsService.UnitsServiceBase
{
    private readonly ILogger<UnitsServiceImpl> _logger;

    private readonly IUnitsDAL _unitsDAL;

    public UnitsServiceImpl(
        ILogger<UnitsServiceImpl> logger,
        IUnitsDAL unitsDAL)
    {
        _logger = logger;
        _unitsDAL = unitsDAL;
    }
    public override async Task<Unit> Get(GetUnitRequest request, ServerCallContext context)
    {
        var entity = await _unitsDAL.GetAsync(request.Id, request.UnitsConvertParams);

        return entity;
    }

    public override async Task<GetUnitsResponse> GetByFilter(GetUnitsRequest request, ServerCallContext context)
    {
        var entities = (await _unitsDAL.GetAsync(request.UnitsSearchParams, request.UnitsConvertParams)).Objects;

        return new GetUnitsResponse { Units = { entities } };
    }

    public override async Task<Empty> CreateOrUpdate(Unit entity, ServerCallContext context)
    {
        await _unitsDAL.AddOrUpdateAsync(entity);

        return new Empty();
    }

    public override async Task<Empty> Delete(DeleteUnitRequest request, ServerCallContext context)
    {
        await _unitsDAL.DeleteAsync(request.Id);

        return new Empty();
    }
}
