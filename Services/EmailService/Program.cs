using Tools.RabbitMQ.Extensions;
using EmailService.BackgroundServices;
using EmailService.BL.Extensions;
using EmailService.DAL.Extensions;
using Common.Configurations;
using Tools.Encryption;
using Tools.Redis;

var builder = WebApplication.CreateBuilder(args);


var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

builder.Services.Configure<AesEncryptionConfiguration>(builder.Configuration.GetSection("AesEncryptionSettings"));
builder.Services.Configure<SmtpConfiguration>(builder.Configuration.GetSection("SmtpSettings"));

var redisConfiguration = builder.Configuration.GetConnectionString("Redis")
                        ?? throw new InvalidOperationException("Redis connection string is missing.");
RedisConnectionFactory.Initialize(redisConfiguration);
builder.Services.AddSingleton<IRedisCache>(provider =>
{
    var redisConnection = RedisConnectionFactory.GetConnection();
    return new RedisCache(redisConnection);
});

builder.Services.AddScoped<AesEncryption>();

builder.Services.AddMessageBrokers(builder.Configuration, environment);
builder.Services.AddBusinessLogicLayer();
builder.Services.AddDataAccessLayer(builder.Configuration, environment);
builder.Services.AddHostedService<EmailProcessingService>();
builder.Services.AddHostedService<SendEmailProcessingService>();

var app = builder.Build();

app.Run();
