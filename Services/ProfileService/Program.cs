using Common.Configurations;
using ProfileService.DAL.Implementations.Extensions;
using ProfileService.Extensions;
using Tools.RabbitMQ.Extensions;
using Tools.AWS3.Extensions;
using Tools.Redis;
using Common.Constants;
using Serilog;
using LoggerConfiguration = Tools.ElasticSearch.LoggerConfiguration;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(LoggerConfiguration.ConfigureLogger(ServiceNames.ProfileService, builder.Configuration));

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(PortsConstants.ProfileServiceHttpPort, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1;
    });

    options.ListenAnyIP(PortsConstants.ProfileServiceGrpcPort, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
    });
});

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

builder.Services.AddDataAccessLayer(builder.Configuration, environment!);
builder.Services.UseAWS3(builder.Configuration);
builder.Services.ConfigureGrpcServices();
builder.Services.ConfigureGrpcClients();
builder.Services.AddMessageBrokers(builder.Configuration, environment);

var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

var redisConfiguration = builder.Configuration.GetConnectionString("Redis")
                        ?? throw new InvalidOperationException("Redis connection string is missing.");
RedisConnectionFactory.Initialize(redisConfiguration);
builder.Services.AddSingleton<IRedisCache>(provider =>
{
    var redisConnection = RedisConnectionFactory.GetConnection();
    return new RedisCache(redisConnection);
});


var app = builder.Build();

app.MapGrpcEndpoints();

app.Run();