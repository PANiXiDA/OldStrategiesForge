using Common.Configurations;
using ProfileService.DAL.Implementations.Extensions;
using ProfileService.Extensions;
using Tools.RabbitMQ.Extensions;
using Tools.Redis;

var builder = WebApplication.CreateBuilder(args);

PortsConfiguration.ConfigurePort();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(PortsConfiguration.HttpPort, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1;
    });

    options.ListenAnyIP(PortsConfiguration.GrpcPort, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
    });
});

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

builder.Services.AddDataAccessLayer(builder.Configuration, environment!);
builder.Services.ConfigureGrpcServices();
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

if (app.Environment.IsDevelopment())
{
    app.MapGrpcReflectionService();
}

app.Run();
