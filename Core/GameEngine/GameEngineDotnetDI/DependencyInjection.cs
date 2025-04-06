using GameEngine.Implementations;
using GameEngine.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace GameEngineDotnetDI
{
    public static class DependencyInjection
    {
        public static IServiceCollection ResolveDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<IATBCalculator, ATBCalculator>();
            services.AddScoped<IDamageCalculator, DamageCalculator>();
            services.AddScoped<IPathFinderCalculator, PathFinderCalculator>();
            services.AddScoped<IGridGenerator, GridGenerator>();

            return services;
        }
    }
}
