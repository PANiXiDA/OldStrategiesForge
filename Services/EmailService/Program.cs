using Tools.RabbitMQ.Extensions;
using EmailService.BackgroundServices;
using EmailService.BL.Extensions;
using EmailService.DAL.Extensions;
using Common.Configurations;
using Tools.Encryption;

var builder = WebApplication.CreateBuilder(args);


var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

builder.Services.Configure<AesEncryptionConfiguration>(builder.Configuration.GetSection("AesEncryptionSettings"));
builder.Services.Configure<SmtpConfiguration>(builder.Configuration.GetSection("SmtpSettings"));

builder.Services.AddScoped<AesEncryption>();

builder.Services.AddMessageBrokers(builder.Configuration, environment);
builder.Services.AddBusinessLogicLayer();
builder.Services.AddDataAccessLayer(builder.Configuration, environment);
builder.Services.AddHostedService<EmailProcessingService>();
builder.Services.AddHostedService<SendEmailProcessingService>();

var app = builder.Build();

app.Run();
