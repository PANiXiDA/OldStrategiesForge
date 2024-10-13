using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Tools.RabbitMQ.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMessageBrokers(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IRabbitMQClient>(config =>
            {
                var hostname = configuration["RabbitMQ"];
                var logger = config.GetRequiredService<ILogger<RabbitMQClient>>();
                return new RabbitMQClient(hostname!, logger);
            });

            return services;
        }
    }
}
