using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Tools.RabbitMQ.Extensions;

public static class DependencyInjection
{
    private const string rabbitMq = "RabbitMQ";
    public static IServiceCollection AddMessageBrokers(this IServiceCollection services, IConfiguration configuration, string? environment)
    {
        services.AddSingleton<IRabbitMQClient>(config =>
        {
            var hostname = string.Empty;

            if (environment != "Development")
            {
                hostname = Environment.GetEnvironmentVariable(rabbitMq);
            }
            else
            {
                hostname = configuration[rabbitMq];
            }

            var logger = config.GetRequiredService<ILogger<RabbitMQClient>>();

            return new RabbitMQClient(hostname!, logger);
        });

        return services;
    }
}
