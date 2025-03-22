using Sessions.Gen;
using GamesService.DAL.Interfaces;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Games.Entities.Gen;

namespace GamesService.Services;

public class SessionsServiceImpl : SessionsService.SessionsServiceBase
{
    private readonly ILogger<SessionsServiceImpl> _logger;

    private readonly ISessionsDAL _sessionsDAL;

    public SessionsServiceImpl(
        ILogger<SessionsServiceImpl> logger,
        ISessionsDAL sessionsDAL)
    {
        _logger = logger;
        _sessionsDAL = sessionsDAL;
    }

    public override async Task<Session> Get(GetSessionRequest request, ServerCallContext context)
    {
        var entity = await _sessionsDAL.GetAsync(Guid.Parse(request.Id), request.SessionsConvertParams);

        return entity;
    }

    public override async Task<GetSessionsResponse> GetByFilter(GetSessionsRequest request, ServerCallContext context)
    {
        var entities = (await _sessionsDAL.GetAsync(request.SessionsSearchParams, request.SessionsConvertParams)).Objects;

        return new GetSessionsResponse { Sessions = { entities } };
    }

    public override async Task<Empty> CreateOrUpdate(Session entity, ServerCallContext context)
    {
        await _sessionsDAL.AddOrUpdateAsync(entity);

        return new Empty();
    }

    public override async Task<Empty> Delete(DeleteSessionRequest request, ServerCallContext context)
    {
        await _sessionsDAL.DeleteAsync(Guid.Parse(request.Id));

        return new Empty();
    }
}
