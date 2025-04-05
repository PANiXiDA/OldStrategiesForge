using GamePlayService.Handlers.Implementations;
using GamePlayService.Handlers.Interfaces;

namespace GamePlayService.Handlers.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddHandlers(this IServiceCollection services)
    {
        services.AddScoped<IConnectionHandler, ConnectionHandler>();
        services.AddScoped<IDeploymentHandler, DeploymentHandler>();
        services.AddScoped<ISurrenderHandler, SurrenderHandler>();
        services.AddScoped<ICommandHandler, CommandHandler>();

        return services;
    }
}
