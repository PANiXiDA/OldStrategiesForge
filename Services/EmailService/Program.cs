using Tools.RabbitMQ.Extensions;
using EmailService.BackgroundServices;
using EmailService.BL.Extensions;
using EmailService.DAL.Extensions;

var builder = WebApplication.CreateBuilder(args);


var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

builder.Services.AddMessageBrokers(builder.Configuration, environment);
builder.Services.AddBusinessLogicLayer();
builder.Services.AddDataAccessLayer(builder.Configuration, environment);
builder.Services.AddHostedService<EmailProcessingService>();

var app = builder.Build();

app.Run();
