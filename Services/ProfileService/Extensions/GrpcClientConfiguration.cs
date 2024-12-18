using Common.Configurations;
using Common.Constants;
using ImageService.S3Images.Gen;

namespace ProfileService.Extensions;

public static class GrpcClientConfiguration
{
    public static void ConfigureGrpcClients(this IServiceCollection services)
    {
        GrpcConfiguration.ConfigureGrpcClient<S3Images.S3ImagesClient>(services, ServiceNames.ImagesService);
    }
}