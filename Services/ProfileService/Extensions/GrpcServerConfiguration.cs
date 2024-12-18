using ProfileService.Services;

namespace ProfileService.Extensions;

public static class GrpcServerConfiguration
{
    public static void MapGrpcEndpoints(this WebApplication app)
    {
        app.MapGrpcService<PlayersServiceImpl>();
        app.MapGrpcService<AuthServiceImpl>();
        app.MapGrpcService<AvatarServiceImpl>();
        app.MapGrpcService<FramesServiceImpl>();

        if (app.Environment.IsDevelopment())
        {
            app.MapGrpcReflectionService();
        }
    }
}
