using Games.Entities.Gen;
using Games.Gen;
using GamesService.DAL.Interfaces;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace GamesService.Services;

public class GamesServiceImpl : Games.Gen.GamesService.GamesServiceBase
{
    private readonly ILogger<GamesServiceImpl> _logger;

    private readonly IGamesDAL _gamesDAL;

    public GamesServiceImpl(
        ILogger<GamesServiceImpl> logger,
        IGamesDAL gamesDAL)
    {
        _logger = logger;
        _gamesDAL = gamesDAL;
    }

    public override async Task<Game> Get(GetGameRequest request, ServerCallContext context)
    {
        var entity = await _gamesDAL.GetAsync(Guid.Parse(request.Id), request.GamesConvertParams);

        return entity;
    }

    public override async Task<GetGamesResponse> GetByFilter(GetGamesRequest request, ServerCallContext context)
    {
        var entities = (await _gamesDAL.GetAsync(request.GamesSearchParams, request.GamesConvertParams)).Objects;

        return new GetGamesResponse { Games = { entities } };
    }

    public override async Task<Empty> CreateOrUpdate(Game entity, ServerCallContext context)
    {
        await _gamesDAL.AddOrUpdateAsync(entity);

        return new Empty();
    }

    public override async Task<Empty> Delete(DeleteGameRequest request, ServerCallContext context)
    {
        await _gamesDAL.DeleteAsync(Guid.Parse(request.Id));

        return new Empty();
    }

    public override async Task<Empty> Close(CloseGameRequest request, ServerCallContext context)
    {
        await _gamesDAL.CloseAsync(Guid.Parse(request.Id));

        return new Empty();
    }
}
