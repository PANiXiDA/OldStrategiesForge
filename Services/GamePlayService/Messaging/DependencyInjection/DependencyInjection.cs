using GamePlayService.Messaging.Implementations;
using GamePlayService.Messaging.Interfaces;

namespace GamePlayService.Messaging.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddMessaging(this IServiceCollection services)
    {
        services.AddScoped<IMessageTasks, MessageTasks>();
        services.AddScoped<IMessageSender, MessageSender>();

        return services;
    }
}
