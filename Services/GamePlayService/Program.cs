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
using Hangfire;
using Hangfire.Redis.StackExchange;
using Common.Constants;
using System.Net.Sockets;
using Common.Converters;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<JwtSettings>>().Value);
builder.Services.AddSingleton<JwtHelper>();

builder.Services.AddSingleton(new UdpClient(PortsConstants.GamePlayServicePort));

builder.Services.ConfigureGrpcClients();
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

var jsonSettings = new JsonSerializerSettings();
jsonSettings.Converters.Add(new IPEndPointNewtonsoftConverter()); // TODO найти альтернативу для сериализации IpEndpoint в HangFire через System.Text.Json и выпилить это

builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseRedisStorage(builder.Configuration.GetConnectionString("Redis"), new RedisStorageOptions())
    .UseSerializerSettings(jsonSettings)
);

builder.Services.AddHangfireServer(options =>
{
    options.SchedulePollingInterval = TimeSpan.FromSeconds(5);
});

builder.Services.AddHostedService<GamePlayServiceImpl>();

var app = builder.Build();

app.UseHangfireDashboard();

app.Run();
