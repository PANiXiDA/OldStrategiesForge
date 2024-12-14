using Profile.Auth.Gen;
using Profile.Players.Gen;
using Profile.Avatar.Gen;
using Profile.Frames.Gen;

namespace APIGateway.Extensions;

public static class GrpcClientConfiguration
{
    public static void ConfigureGrpcClients(this IServiceCollection services)
    {
        ConfigureGrpcClient<ProfilePlayers.ProfilePlayersClient>(services, "PROFILE_SERVICE_URL");
        ConfigureGrpcClient<ProfileAuth.ProfileAuthClient>(services, "PROFILE_SERVICE_URL");
        ConfigureGrpcClient<ProfileAvatars.ProfileAvatarsClient>(services, "PROFILE_SERVICE_URL");
        ConfigureGrpcClient<ProfileFrames.ProfileFramesClient>(services, "PROFILE_SERVICE_URL");
    }

    private static void ConfigureGrpcClient<TClient>(IServiceCollection services, string environmentVariableName) where TClient : class
    {
        var url = Environment.GetEnvironmentVariable(environmentVariableName);
        if (url == null)
        {
            throw new InvalidOperationException($"{environmentVariableName} is not set.");
        }

        services.AddGrpcClient<TClient>(o => o.Address = new Uri(url));
    }
}
