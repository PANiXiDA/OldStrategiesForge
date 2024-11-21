using EmailService.BL.BL.Interfaces;
using EmailService.BL.BL.Standard;
using EmailService.BL.Dto.Handlers;

namespace EmailService.BL.Extensions;

internal static class DependencyInjection
{
    internal static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services)
    {
        services.AddScoped<INotificationSubscribersBL, NotificationSubscribersBL>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(NotificationSubscriberCommandHandler).Assembly));

        services.AddScoped<IEmailSenderBL, EmailSenderBL>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ProcessEmailSenderHandler).Assembly));

        return services;
    }
}
