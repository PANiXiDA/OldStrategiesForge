using EmailService.DAL.DAL.DbModels;
using EmailService.DAL.DAL.Interfaces;
using EmailService.DAL.DAL.SQL;
using Microsoft.EntityFrameworkCore;

namespace EmailService.DAL.Extensions;

internal static class DependencyInjection
{
    internal static IServiceCollection AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration, string? environment)
    {
        if (environment != "Development")
        {
            var connectionString =
                $"Host={Environment.GetEnvironmentVariable("DB_HOST")};" +
                $"Port={Environment.GetEnvironmentVariable("DB_PORT")};" +
                $"Database={Environment.GetEnvironmentVariable("POSTGRES_DB")};" +
                $"Username={Environment.GetEnvironmentVariable("DB_USER")};" +
                $"Password={Environment.GetEnvironmentVariable("DB_PASSWORD")}";

            services.AddDbContext<DefaultDbContext>(options => options.UseNpgsql(connectionString));
        }
        else
        {
            services.AddDbContext<DefaultDbContext>(config => config.UseNpgsql(configuration["ConnectionStrings:DefaultConnectionString"]));
        }

        services.AddScoped<INotificationSubscribersDAL, NotificationSubscribersDAL>();

        return services;
    }
}
