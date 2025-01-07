using Profile.Auth.Gen;
using Profile.Players.Gen;
using Profile.Avatar.Gen;
using Profile.Frames.Gen;
using Common.Configurations;
using Common.Constants;
using ImageService.S3Images.Gen;
using Global.Chat.Gen;
using Matchmaking.Gen;

namespace APIGateway.Extensions;

public static class GrpcClientConfiguration
{
    public static void ConfigureGrpcClients(this IServiceCollection services)
    {
        GrpcConfiguration.ConfigureGrpcClient<ProfilePlayers.ProfilePlayersClient>(services, ServiceNames.ProfileService);
        GrpcConfiguration.ConfigureGrpcClient<ProfileAuth.ProfileAuthClient>(services, ServiceNames.ProfileService);
        GrpcConfiguration.ConfigureGrpcClient<ProfileAvatars.ProfileAvatarsClient>(services, ServiceNames.ProfileService);
        GrpcConfiguration.ConfigureGrpcClient<ProfileFrames.ProfileFramesClient>(services, ServiceNames.ProfileService);
        GrpcConfiguration.ConfigureGrpcClient<S3Images.S3ImagesClient>(services, ServiceNames.ImagesService);
        GrpcConfiguration.ConfigureGrpcClient<GlobalChat.GlobalChatClient>(services, ServiceNames.ChatsService);
        GrpcConfiguration.ConfigureGrpcClient<GameMatchmaking.GameMatchmakingClient>(services, ServiceNames.GamesService);
    }
}
