using Tools.RabbitMQ.Extensions;
using EmailService.BackgroundServices;
using EmailService.BL.Extensions;
using EmailService.DAL.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMessageBrokers(builder.Configuration);
builder.Services.AddBusinessLogicLayer();
builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddHostedService<EmailProcessingService>();

var app = builder.Build();

app.Run();
