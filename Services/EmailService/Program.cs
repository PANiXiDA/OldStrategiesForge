using Tools.RabbitMQ.Extensions;
using EmailService.BackgroundServices;
using EmailService.BL.Extensions;
using EmailService.DAL.Extensions;
using Common.Configurations;
using Tools.Encryption;
using Tools.Redis;
using Common.Constants;
using Serilog;
using LoggerConfiguration = Tools.ElasticSearch.LoggerConfiguration;
using StackExchange.Redis;
using RedLockNet;
using RedLockNet.SERedis.Configuration;
using RedLockNet.SERedis;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(LoggerConfiguration.ConfigureLogger(ServiceNames.EmailService, builder.Configuration));

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

builder.Services.Configure<AesEncryptionConfiguration>(builder.Configuration.GetSection("AesEncryptionSettings"));
builder.Services.Configure<SmtpConfiguration>(builder.Configuration.GetSection("SmtpSettings"));

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

builder.Services.AddScoped<AesEncryption>();

builder.Services.AddMessageBrokers(builder.Configuration, environment);
builder.Services.AddBusinessLogicLayer();
builder.Services.AddDataAccessLayer(builder.Configuration, environment);
builder.Services.AddHostedService<EmailProcessingService>();
builder.Services.AddHostedService<SendEmailProcessingService>();

var app = builder.Build();

app.Run();
