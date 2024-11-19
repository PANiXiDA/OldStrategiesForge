using Profile.Auth.Gen;
using Players.Backend.Gen;

namespace APIGateway.Extensions;

public static class GrpcClientConfiguration
{
    public static void ConfigureGrpcClients(this IServiceCollection services)
    {
        ConfigureGrpcClient<PlayersBackend.PlayersBackendClient>(services, "PROFILE_BACKEND_URL");
        ConfigureGrpcClient<ProfileAuth.ProfileAuthClient>(services, "PROFILE_BACKEND_URL");
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
