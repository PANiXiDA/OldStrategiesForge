using GameData.Competencies.Gen;
using GameData.Entities.Gen;
using GameDataService.DAL.Interfaces;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace GameDataService.Services;

public class CompetenciesServiceImpl : CompetenciesService.CompetenciesServiceBase
{
    private readonly ILogger<CompetenciesServiceImpl> _logger;

    private readonly ICompetenciesDAL _competenciesDAL;

    public CompetenciesServiceImpl(
        ILogger<CompetenciesServiceImpl> logger,
        ICompetenciesDAL competenciesDAL)
    {
        _logger = logger;
        _competenciesDAL = competenciesDAL;
    }

    public override async Task<Competence> Get(GetCompetenceRequest request, ServerCallContext context)
    {
        var entity = await _competenciesDAL.GetAsync(request.Id, request.CompetenciesConvertParams);

        return entity;
    }

    public override async Task<GetCompetenciesResponse> GetByFilter(GetCompetenciesRequest request, ServerCallContext context)
    {
        var entities = (await _competenciesDAL.GetAsync(request.CompetenciesSearchParams, request.CompetenciesConvertParams)).Objects;

        return new GetCompetenciesResponse { Competencies = { entities } };
    }

    public override async Task<Empty> CreateOrUpdate(Competence entity, ServerCallContext context)
    {
        await _competenciesDAL.AddOrUpdateAsync(entity);

        return new Empty();
    }

    public override async Task<Empty> Delete(DeleteCompetenceRequest request, ServerCallContext context)
    {
        await _competenciesDAL.DeleteAsync(request.Id);

        return new Empty();
    }
}