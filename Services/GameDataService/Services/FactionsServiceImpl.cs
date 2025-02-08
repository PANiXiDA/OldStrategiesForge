using GameData.Factions.Gen;
using Grpc.Core;

namespace GameDataService.Services;

public class FactionsServiceImpl : Factions.FactionsBase
{
    private readonly ILogger<FactionsServiceImpl> _logger;

    public FactionsServiceImpl(
        ILogger<FactionsServiceImpl> logger)
    {
        _logger = logger;
    }

    public override async Task<GetFactionResponse> GetFaction(GetFactionRequest request, ServerCallContext context)
    {
        return new GetFactionResponse();
    }
}
