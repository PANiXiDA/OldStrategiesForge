using GameData.ArtefactSetBonuses.Gen;
using GameData.Entities.Gen;
using GameDataService.DAL.Interfaces;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
namespace GameDataService.Services;

public class ArtefactSetBonusesServiceImpl : ArtefactSetBonusesService.ArtefactSetBonusesServiceBase
{
    private readonly ILogger<ArtefactSetBonusesServiceImpl> _logger;

    private readonly IArtefactSetBonusesDAL _artefactSetBonusesDAL;

    public ArtefactSetBonusesServiceImpl(
        ILogger<ArtefactSetBonusesServiceImpl> logger,
        IArtefactSetBonusesDAL artefactSetBonusesDAL)
    {
        _logger = logger;
        _artefactSetBonusesDAL = artefactSetBonusesDAL;
    }
    public override async Task<ArtefactSetBonus> Get(GetArtefactSetBonusRequest request, ServerCallContext context)
    {
        var entity = await _artefactSetBonusesDAL.GetAsync(request.Id, request.ArtefactSetBonusesConvertParams);

        return entity;
    }

    public override async Task<GetArtefactSetBonusesResponse> GetByFilter(GetArtefactSetBonusesRequest request, ServerCallContext context)
    {
        var entities = (await _artefactSetBonusesDAL.GetAsync(request.ArtefactSetBonusesSearchParams, request.ArtefactSetBonusesConvertParams)).Objects;

        return new GetArtefactSetBonusesResponse { ArtefactSetBonuses = { entities } };
    }

    public override async Task<Empty> CreateOrUpdate(ArtefactSetBonus entity, ServerCallContext context)
    {
        await _artefactSetBonusesDAL.AddOrUpdateAsync(entity);

        return new Empty();
    }

    public override async Task<Empty> Delete(DeleteArtefactSetBonusRequest request, ServerCallContext context)
    {
        await _artefactSetBonusesDAL.DeleteAsync(request.Id);

        return new Empty();
    }
}