using GameDataService.Services;

namespace GameDataService.Extensions;

public static class GrpcServerConfiguration
{
    public static void MapGrpcEndpoints(this WebApplication app)
    {
        app.MapGrpcService<FactionsServiceImpl>();
        app.MapGrpcService<SubfactionsServiceImpl>();
        app.MapGrpcService<HeroesServiceImpl>();
        app.MapGrpcService<HeroClassesServiceImpl>();
        app.MapGrpcService<UnitsServiceImpl>();
        app.MapGrpcService<ArtefactsServiceImpl>();
        app.MapGrpcService<ArtefactSetsServiceImpl>();
        app.MapGrpcService<ArtefactSetBonusesServiceImpl>();
        app.MapGrpcService<CompetenciesServiceImpl>();
        app.MapGrpcService<SkillsServiceImpl>();
        app.MapGrpcService<SpellsServiceImpl>();
        app.MapGrpcService<AbilitiesServiceImpl>();
        app.MapGrpcService<EffectsServiceImpl>();

        if (app.Environment.IsDevelopment())
        {
            app.MapGrpcReflectionService();
        }
    }
}