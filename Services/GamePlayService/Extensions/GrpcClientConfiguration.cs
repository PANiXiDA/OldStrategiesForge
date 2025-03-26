using Common.Configurations;
using Common.Constants;
using Games.Gen;
using PlayerBuilds.Gen;
using Sessions.Gen;

namespace GamePlayService.Extensions;

public static class GrpcClientConfiguration
{
    public static void ConfigureGrpcClients(this IServiceCollection services)
    {
        GrpcConfiguration.ConfigureGrpcClient<GamesService.GamesServiceClient>(services, ServiceNames.GamesService);
        GrpcConfiguration.ConfigureGrpcClient<SessionsService.SessionsServiceClient>(services, ServiceNames.GamesService);

        GrpcConfiguration.ConfigureGrpcClient<PlayerBuildsService.PlayerBuildsServiceClient>(services, ServiceNames.PlayerBuildsService);
    }
}
