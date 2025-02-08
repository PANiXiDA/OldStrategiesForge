using GameDataService.Services;

namespace GameDataService.Extensions;

public static class GrpcServerConfiguration
{
    public static void MapGrpcEndpoints(this WebApplication app)
    {
        app.MapGrpcService<FactionsServiceImpl>();

        if (app.Environment.IsDevelopment())
        {
            app.MapGrpcReflectionService();
        }
    }
}