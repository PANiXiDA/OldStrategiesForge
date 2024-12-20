using Common.Configurations;
using Common.Constants;
using ImageService.S3Images.Gen;
using Profile.Players.Gen;

namespace ChatsService.Extensions;

public static class GrpcClientConfiguration
{
    public static void ConfigureGrpcClients(this IServiceCollection services)
    {
        GrpcConfiguration.ConfigureGrpcClient<ProfilePlayers.ProfilePlayersClient>(services, ServiceNames.ProfileService);
        GrpcConfiguration.ConfigureGrpcClient<S3Images.S3ImagesClient>(services, ServiceNames.ImagesService);
    }
}