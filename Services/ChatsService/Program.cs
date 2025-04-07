using ChatsService.DAL.Implementations.Extensions;
using ChatsService.Extensions;
using Common.Configurations;
using Common.Constants;
using Serilog;
using LoggerConfiguration = Tools.ElasticSearch.LoggerConfiguration;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(LoggerConfiguration.ConfigureLogger(ServiceNames.ChatsService, builder.Configuration));

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(PortsConstants.ChatsServiceHttpPort, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1;
    });

    options.ListenAnyIP(PortsConstants.ChatsServiceGrpcPort, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
    });
});

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

builder.Services.AddDataAccessLayer(builder.Configuration, environment!);
builder.Services.ConfigureGrpcServices();
builder.Services.ConfigureGrpcClients();

var app = builder.Build();

app.MapGrpcEndpoints();

app.Run();
