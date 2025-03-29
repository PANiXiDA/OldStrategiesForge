using Common.Configurations;
using GamePlayService.Extensions;
using GamePlayService.Extensions.Helpers;
using GamePlayService.Services;
using Microsoft.Extensions.Options;
using GameEngineDotnetDI;
using GamePlayService.BL.BL.DependencyInjection;
using Tools.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<JwtSettings>>().Value);
builder.Services.AddSingleton<JwtHelper>();

builder.Services.ConfigureGrpcClients();
builder.Services.ResolveDependencyInjection();
builder.Services.AddBusinessLogicLayer();

var redisConfiguration = builder.Configuration.GetConnectionString("Redis")
                        ?? throw new InvalidOperationException("Redis connection string is missing.");
RedisConnectionFactory.Initialize(redisConfiguration);
builder.Services.AddSingleton<IRedisCache>(provider =>
{
    var redisConnection = RedisConnectionFactory.GetConnection();
    return new RedisCache(redisConnection);
});

builder.Services.AddHostedService<GamePlayServiceImpl>();

var app = builder.Build();

app.Run();