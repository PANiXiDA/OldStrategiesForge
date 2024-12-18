using Profile.Auth.Gen;
using Profile.Players.Gen;
using Profile.Avatar.Gen;
using Profile.Frames.Gen;
using Common.Configurations;
using Common.Constants;

namespace APIGateway.Extensions;

public static class GrpcClientConfiguration
{
    public static void ConfigureGrpcClients(this IServiceCollection services)
    {
        GrpcConfiguration.ConfigureGrpcClient<ProfilePlayers.ProfilePlayersClient>(services, ServiceNames.ProfileService);
        GrpcConfiguration.ConfigureGrpcClient<ProfileAuth.ProfileAuthClient>(services, ServiceNames.ProfileService);
        GrpcConfiguration.ConfigureGrpcClient<ProfileAvatars.ProfileAvatarsClient>(services, ServiceNames.ProfileService);
        GrpcConfiguration.ConfigureGrpcClient<ProfileFrames.ProfileFramesClient>(services, ServiceNames.ProfileService);
    }
}
