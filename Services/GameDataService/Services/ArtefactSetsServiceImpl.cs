using GameData.ArtefactSets.Gen;
using GameData.Entities.Gen;
using GameDataService.DAL.Interfaces;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace GameDataService.Services;

public class ArtefactSetsServiceImpl : ArtefactSetsService.ArtefactSetsServiceBase
{
    private readonly ILogger<ArtefactSetsServiceImpl> _logger;

    private readonly IArtefactSetsDAL _artefactSetsDAL;

    public ArtefactSetsServiceImpl(
        ILogger<ArtefactSetsServiceImpl> logger,
        IArtefactSetsDAL artefactSetsDAL)
    {
        _logger = logger;
        _artefactSetsDAL = artefactSetsDAL;
    }

    public override async Task<ArtefactSet> Get(GetArtefactSetRequest request, ServerCallContext context)
    {
        var entity = await _artefactSetsDAL.GetAsync(request.Id, request.ArtefactSetsConvertParams);

        return entity;
    }

    public override async Task<GetArtefactSetsResponse> GetByFilter(GetArtefactSetsRequest request, ServerCallContext context)
    {
        var entities = (await _artefactSetsDAL.GetAsync(request.ArtefactSetsSearchParams, request.ArtefactSetsConvertParams)).Objects;

        return new GetArtefactSetsResponse { ArtefactSets = { entities } };
    }

    public override async Task<Empty> CreateOrUpdate(ArtefactSet entity, ServerCallContext context)
    {
        await _artefactSetsDAL.AddOrUpdateAsync(entity);

        return new Empty();
    }

    public override async Task<Empty> Delete(DeleteArtefactSetRequest request, ServerCallContext context)
    {
        await _artefactSetsDAL.DeleteAsync(request.Id);

        return new Empty();
    }
}