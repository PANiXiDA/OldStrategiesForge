using ChatsService.Services;

namespace ChatsService.Extensions;

public static class GrpcServerConfiguration
{
    public static void MapGrpcEndpoints(this WebApplication app)
    {
        app.MapGrpcService<GlobalChatServiceImpl>();

        if (app.Environment.IsDevelopment())
        {
            app.MapGrpcReflectionService();
        }
    }
}
