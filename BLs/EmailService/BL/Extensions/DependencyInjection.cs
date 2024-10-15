using EmailService.BL.BL.Interfaces;
using EmailService.BL.BL.Standard;

namespace EmailService.BL.Extensions;

internal static class DependencyInjection
{
    internal static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services)
    {
        services.AddScoped<INotificationSubscribersBL, NotificationSubscribersBL>();

        return services;

    }
}
