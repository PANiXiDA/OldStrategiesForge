using Microsoft.EntityFrameworkCore;
using ProfileDatabaseService.DAL.DbModels;
using ProfileDatabaseService.DAL.Interfaces;

namespace ProfileDatabaseService.DAL.Implementations.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration, string environment)
    {
        if (environment == "Development")
        {
            services.AddDbContext<DefaultDbContext>(config => config.UseNpgsql(configuration["ConnectionStrings:DefaultConnectionString"]));
        }
        else
        {
            var connectionString =
                    $"Host={Environment.GetEnvironmentVariable("DB_HOST")};" +
                    $"Port={Environment.GetEnvironmentVariable("DB_PORT")};" +
                    $"Database={Environment.GetEnvironmentVariable("POSTGRES_DB")};" +
                    $"Username={Environment.GetEnvironmentVariable("DB_USER")};" +
                    $"Password={Environment.GetEnvironmentVariable("DB_PASSWORD")}";

            services.AddDbContext<DefaultDbContext>(options => options.UseNpgsql(connectionString));
        }
        services.AddScoped<IPlayersDAL, PlayersDAL>();

        return services;
    }
}
