using Common.Configurations;
using Common.Constants;
using GameDataService.DAL.Implementations.Extensions;
using GameDataService.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(PortsConstants.GameDataServiceHttpPort, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1;
    });

    options.ListenAnyIP(PortsConstants.GameDataServiceGrpcPort, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
    });
});

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

builder.Services.AddDataAccessLayer(builder.Configuration, environment!);
builder.Services.ConfigureGrpcServices();

var app = builder.Build();

app.MapGrpcEndpoints();

app.Run();