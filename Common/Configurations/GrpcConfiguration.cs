using Common.Constants;
using Common.Extensions;
using Common.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Configurations;

public static class GrpcConfiguration
{
    private const string HttpScheme = "http://";

    public static void ConfigureGrpcClient<TClient>(IServiceCollection services, string serviceName) where TClient : class
    {
        var (_, grpcPort) = ServicePorts.GetPorts(serviceName);

        string url = string.Empty;
        if (EnvironmentHelper.IsDevelopment)
        {
            url = $"{HttpScheme}{ServiceNames.Localhost}:{grpcPort}";
        }
        else
        {
            url = $"{HttpScheme}{serviceName}:{grpcPort}";
        }

        services.AddGrpcClient<TClient>(o => o.Address = new Uri(url));
    }

    public static void ConfigureGrpcServices(this IServiceCollection services)
    {
        services.AddSingleton<ExceptionInterceptor>();
        services.AddGrpc(options =>
        {
            options.Interceptors.Add<ExceptionInterceptor>();
        });
        services.AddGrpcReflection();
    }
}