using GamesService.Services;

namespace GamesService.Extensions;

public static class GrpcServerConfiguration
{
    public static void MapGrpcEndpoints(this WebApplication app)
    {
        app.MapGrpcService<GamesServiceImpl>();

        if (app.Environment.IsDevelopment())
        {
            app.MapGrpcReflectionService();
        }
    }
}
