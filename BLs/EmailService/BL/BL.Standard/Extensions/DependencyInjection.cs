using EmailService.BL.BL.Interfaces;

namespace EmailService.BL.BL.Standard.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services)
        {
            services.AddScoped<INotificationSubscribersBL, NotificationSubscribersBL>();

            return services;

        }
    }
}
