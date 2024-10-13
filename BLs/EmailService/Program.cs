using Tools.RabbitMQ.Extensions;
using EmailService.BackgroundServices;
using EmailService.BL.BL.Standard.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMessageBrokers(builder.Configuration);
builder.Services.AddBusinessLogicLayer();
builder.Services.AddHostedService<EmailProcessingService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
