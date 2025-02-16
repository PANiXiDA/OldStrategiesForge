using GameData.Artefacts.Gen;
using GameData.Entities.Gen;
using GameDataService.DAL.Interfaces;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace GameDataService.Services;

public class ArtefactsServiceImpl : ArtefactsService.ArtefactsServiceBase
{
    private readonly ILogger<ArtefactsServiceImpl> _logger;

    private readonly IArtefactsDAL _artefactsDAL;

    public ArtefactsServiceImpl(
        ILogger<ArtefactsServiceImpl> logger,
        IArtefactsDAL artefactsDAL)
    {
        _logger = logger;
        _artefactsDAL = artefactsDAL;
    }

    public override async Task<Artefact> Get(GetArtefactRequest request, ServerCallContext context)
    {
        var entity = await _artefactsDAL.GetAsync(request.Id, request.ArtefactsConvertParams);

        return entity;
    }

    public override async Task<GetArtefactsResponse> GetByFilter(GetArtefactsRequest request, ServerCallContext context)
    {
        var entities = (await _artefactsDAL.GetAsync(request.ArtefactsSearchParams, request.ArtefactsConvertParams)).Objects;

        return new GetArtefactsResponse { Artefacts = { entities } };
    }

    public override async Task<Empty> CreateOrUpdate(Artefact entity, ServerCallContext context)
    {
        await _artefactsDAL.AddOrUpdateAsync(entity);

        return new Empty();
    }

    public override async Task<Empty> Delete(DeleteArtefactRequest request, ServerCallContext context)
    {
        await _artefactsDAL.DeleteAsync(request.Id);

        return new Empty();
    }
}
