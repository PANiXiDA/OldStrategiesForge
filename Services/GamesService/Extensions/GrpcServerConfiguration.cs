using GamesService.Services;

namespace GamesService.Extensions;

public static class GrpcServerConfiguration
{
    public static void MapGrpcEndpoints(this WebApplication app)
    {
        app.MapGrpcService<MatchmakingServiceImpl>();
        app.MapGrpcService<GamesServiceImpl>();
        app.MapGrpcService<SessionsServiceImpl>();

        if (app.Environment.IsDevelopment())
        {
            app.MapGrpcReflectionService();
        }
    }
}
