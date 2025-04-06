using PlayerBuildsService.Services;

namespace PlayerBuildsService.Extensions;

public static class GrpcServerConfiguration
{
    public static void MapGrpcEndpoints(this WebApplication app)
    {
        app.MapGrpcService<PlayerBuildsServiceImpl>();

        if (app.Environment.IsDevelopment())
        {
            app.MapGrpcReflectionService();
        }
    }
}
