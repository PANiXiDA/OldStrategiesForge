using Common.Configurations;
using Common.Constants;
using GamesService.DAL.Implementations.Extensions;
using GamesService.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(PortsConstants.GamesServiceHttpPort, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1;
    });

    options.ListenAnyIP(PortsConstants.GamesServiceGrpcPort, listenOptions =>
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