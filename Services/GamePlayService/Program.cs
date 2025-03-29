using Common.Configurations;
using GamePlayService.Extensions;
using GamePlayService.Extensions.Helpers;
using GamePlayService.Services;
using Microsoft.Extensions.Options;
using GameEngineDotnetDI;
using GamePlayService.BL.BL.DependencyInjection;
using Tools.Redis;
using RedLockNet;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis;
using Tools.RabbitMQ.Extensions;
using Hangfire;
using Hangfire.Redis.StackExchange;

var builder = WebApplication.CreateBuilder(args);

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<JwtSettings>>().Value);
builder.Services.AddSingleton<JwtHelper>();

builder.Services.ConfigureGrpcClients();
builder.Services.AddMessageBrokers(builder.Configuration, environment);
builder.Services.ResolveDependencyInjection();
builder.Services.AddBusinessLogicLayer();

var redisConfiguration = builder.Configuration.GetConnectionString("Redis")
                        ?? throw new InvalidOperationException("Redis connection string is missing.");

builder.Services.AddSingleton<IConnectionMultiplexer>(provider =>
{
    return ConnectionMultiplexer.Connect(redisConfiguration);
});

builder.Services.AddSingleton<IRedisCache>(provider =>
{
    var multiplexer = provider.GetRequiredService<IConnectionMultiplexer>();
    return new RedisCache(multiplexer);
});

builder.Services.AddSingleton<IDistributedLockFactory>(provider =>
{
    var multiplexer = provider.GetRequiredService<IConnectionMultiplexer>();
    var multiplexers = new List<RedLockMultiplexer>
    {
        new RedLockMultiplexer(multiplexer)
    };
    return RedLockFactory.Create(multiplexers);
});

builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseRedisStorage(builder.Configuration.GetConnectionString("Redis"), new RedisStorageOptions())
);

builder.Services.AddHangfireServer();

builder.Services.AddHostedService<GamePlayServiceImpl>();

var app = builder.Build();

app.UseHangfireDashboard();

app.Run();
