using ProfileDatabaseService.Services;

namespace ProfileDatabaseService.Extensions;

public static class GrpcServerConfiguration
{
    public static void ConfigureGrpcServices(this IServiceCollection services)
    {
        services.AddGrpc();
        services.AddGrpcReflection();
    }

    public static void MapGrpcEndpoints(this WebApplication app)
    {
        app.MapGrpcService<PlayersDatabaseServiceImpl>();
        app.MapGrpcService<AuthDatabaseServiceImpl>();

        if (app.Environment.IsDevelopment())
        {
            app.MapGrpcReflectionService();
        }
    }
}
