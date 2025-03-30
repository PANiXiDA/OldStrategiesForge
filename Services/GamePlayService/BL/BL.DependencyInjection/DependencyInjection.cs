using GamePlayService.BL.BL.Interfaces;
using GamePlayService.BL.BL.Standard;

namespace GamePlayService.BL.BL.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services)
    {
        services.AddScoped<IConnectionsBL, ConnectionsBL>();
        services.AddScoped<IPlayerBuildsFactory, PlayerBuildsFactory>();
        services.AddScoped<IDeploymentBL, DeploymentBL>();

        return services;
    }
}
