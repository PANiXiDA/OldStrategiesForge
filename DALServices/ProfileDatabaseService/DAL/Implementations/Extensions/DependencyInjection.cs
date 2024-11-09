using Microsoft.EntityFrameworkCore;
using ProfileDatabaseService.DAL.DbModels;
using ProfileDatabaseService.DAL.Interfaces;

namespace ProfileDatabaseService.DAL.Implementations.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DefaultDbContext>(config => config.UseNpgsql(configuration["ConnectionStrings:DefaultConnectionString"]));
        services.AddScoped<IPlayersDAL, PlayersDAL>();

        return services;
    }
}
