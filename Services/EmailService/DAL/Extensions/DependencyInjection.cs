using EmailService.DAL.DAL.DbModels;
using EmailService.DAL.DAL.Interfaces;
using EmailService.DAL.DAL.SQL;
using Microsoft.EntityFrameworkCore;

namespace EmailService.DAL.Extensions;

internal static class DependencyInjection
{
    internal static IServiceCollection AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DefaultDbContext>(config => config.UseNpgsql(configuration["ConnectionStrings:DefaultConnectionString"]));
        services.AddScoped<INotificationSubscribersDAL, NotificationSubscribersDAL>();

        return services;
    }
}
