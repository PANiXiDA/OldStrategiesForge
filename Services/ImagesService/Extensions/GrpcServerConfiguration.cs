using ImagesService.Services;

namespace ImagesService.Extensions;

public static class GrpcServerConfiguration
{
    public static void ConfigureGrpcServices(this IServiceCollection services)
    {
        services.AddSingleton<ExceptionInterceptor>();
        services.AddGrpc(options =>
        {
            options.Interceptors.Add<ExceptionInterceptor>();
        });
        services.AddGrpcReflection();
    }

    public static void MapGrpcEndpoints(this WebApplication app)
    {
        app.MapGrpcService<S3ImagesServiceImpl>();

        if (app.Environment.IsDevelopment())
        {
            app.MapGrpcReflectionService();
        }
    }
}
