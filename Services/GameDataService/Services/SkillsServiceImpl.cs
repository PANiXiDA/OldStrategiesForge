using GameData.Entities.Gen;
using GameData.Skills.Gen;
using GameDataService.DAL.Interfaces;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace GameDataService.Services;

public class SkillsServiceImpl : SkillsService.SkillsServiceBase
{
    private readonly ILogger<SkillsServiceImpl> _logger;

    private readonly ISkillsDAL _skillsDAL;

    public SkillsServiceImpl(
        ILogger<SkillsServiceImpl> logger,
        ISkillsDAL skillsDAL)
    {
        _logger = logger;
        _skillsDAL = skillsDAL;
    }

    public override async Task<Skill> Get(GetSkillRequest request, ServerCallContext context)
    {
        var entity = await _skillsDAL.GetAsync(request.Id, request.SkillsConvertParams);

        return entity;
    }

    public override async Task<GetSkillsResponse> GetByFilter(GetSkillsRequest request, ServerCallContext context)
    {
        var entities = (await _skillsDAL.GetAsync(request.SkillsSearchParams, request.SkillsConvertParams)).Objects;

        return new GetSkillsResponse { Skills = { entities } };
    }

    public override async Task<Empty> CreateOrUpdate(Skill entity, ServerCallContext context)
    {
        await _skillsDAL.AddOrUpdateAsync(entity);

        return new Empty();
    }

    public override async Task<Empty> Delete(DeleteSkillRequest request, ServerCallContext context)
    {
        await _skillsDAL.DeleteAsync(request.Id);

        return new Empty();
    }
}