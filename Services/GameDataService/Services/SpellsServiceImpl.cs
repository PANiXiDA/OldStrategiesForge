using GameData.Entities.Gen;
using GameData.Spells.Gen;
using GameDataService.DAL.Interfaces;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace GameDataService.Services;

public class SpellsServiceImpl : SpellsService.SpellsServiceBase
{
    private readonly ILogger<SpellsServiceImpl> _logger;

    private readonly ISpellsDAL _spellsDAL;

    public SpellsServiceImpl(
        ILogger<SpellsServiceImpl> logger,
        ISpellsDAL spellsDAL)
    {
        _logger = logger;
        _spellsDAL = spellsDAL;
    }

    public override async Task<Spell> Get(GetSpellRequest request, ServerCallContext context)
    {
        var entity = await _spellsDAL.GetAsync(request.Id, request.SpellsConvertParams);

        return entity;
    }

    public override async Task<GetSpellsResponse> GetByFilter(GetSpellsRequest request, ServerCallContext context)
    {
        var entities = (await _spellsDAL.GetAsync(request.SpellsSearchParams, request.SpellsConvertParams)).Objects;

        return new GetSpellsResponse { Spells = { entities } };
    }

    public override async Task<Empty> CreateOrUpdate(Spell entity, ServerCallContext context)
    {
        await _spellsDAL.AddOrUpdateAsync(entity);

        return new Empty();
    }

    public override async Task<Empty> Delete(DeleteSpellRequest request, ServerCallContext context)
    {
        await _spellsDAL.DeleteAsync(request.Id);

        return new Empty();
    }
}
