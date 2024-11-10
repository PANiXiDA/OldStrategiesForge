using Auth.Database.Gen;
using Players.Database.Gen;

namespace ProfileBackendService.Extensions;

public static class GrpcClientConfiguration
{
    public static void ConfigureGrpcClients(this IServiceCollection services)
    {
        ConfigureGrpcClient<PlayersDatabase.PlayersDatabaseClient>(services, "PROFILE_DATABASE_URL");
        ConfigureGrpcClient<AuthDatabase.AuthDatabaseClient>(services, "PROFILE_DATABASE_URL");
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
