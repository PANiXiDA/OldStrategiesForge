using GameDataService.DAL.DbModels;
using GameDataService.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GameDataService.DAL.Implementations.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration, string environment)
    {
        //if (environment == "Development")
        //{
        //    services.AddDbContext<DefaultDbContext>(config => config.UseNpgsql(configuration["ConnectionStrings:DefaultConnectionString"]));
        //}
        //else
        //{
        //    var connectionString =
        //            $"Host={Environment.GetEnvironmentVariable("DB_HOST")};" +
        //            $"Port={Environment.GetEnvironmentVariable("DB_PORT")};" +
        //            $"Database={Environment.GetEnvironmentVariable("POSTGRES_DB")};" +
        //            $"Username={Environment.GetEnvironmentVariable("DB_USER")};" +
        //            $"Password={Environment.GetEnvironmentVariable("DB_PASSWORD")}";

        //    services.AddDbContext<DefaultDbContext>(options => options.UseNpgsql(connectionString));
        //}

        services.AddDbContext<DefaultDbContext>(config => config.UseNpgsql(configuration["ConnectionStrings:DefaultConnectionString"]));

        services.AddScoped<IUnitsDAL, UnitsDAL>();
        services.AddScoped<ISubfactionsDAL, SubfactionsDAL>();
        services.AddScoped<ISpellsDAL, SpellsDAL>();
        services.AddScoped<ISkillsDAL, SkillsDAL>();
        services.AddScoped<IHeroesDAL, HeroesDAL>();
        services.AddScoped<IFactionsDAL, FactionsDAL>();
        services.AddScoped<IHeroClassesDAL, HeroClassesDAL>();
        services.AddScoped<IEffectsDAL, EffectsDAL>();
        services.AddScoped<ICompetenciesDAL, CompetenciesDAL>();
        services.AddScoped<IArtefactSetsDAL, ArtefactSetsDAL>();
        services.AddScoped<IArtefactSetBonusesDAL, ArtefactSetBonusesDAL>();
        services.AddScoped<IArtefactsDAL, ArtefactsDAL>();
        services.AddScoped<IAbilitiesDAL, AbilitiesDAL>();

        return services;
    }
}
