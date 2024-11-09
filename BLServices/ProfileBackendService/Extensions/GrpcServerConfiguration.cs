using ProfileBackendService.Services;

namespace ProfileBackendService.Extensions;

public static class GrpcServerConfiguration
{
    public static void ConfigureGrpcServices(this IServiceCollection services)
    {
        services.AddGrpc();
        services.AddGrpcReflection();
    }

    public static void MapGrpcEndpoints(this WebApplication app)
    {
        app.MapGrpcService<PlayersBackendServiceImpl>();
        app.MapGrpcService<AuthBackendServiceImpl>();

        if (app.Environment.IsDevelopment())
        {
            app.MapGrpcReflectionService();
        }
    }
}
