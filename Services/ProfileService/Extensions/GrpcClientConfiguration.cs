using ImageService.S3Images.Gen;

namespace ProfileService.Extensions;

public static class GrpcClientConfiguration
{
    public static void ConfigureGrpcClients(this IServiceCollection services)
    {
        ConfigureGrpcClient<S3Images.S3ImagesClient>(services, "IMAGES_SERVICE_URL");
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
