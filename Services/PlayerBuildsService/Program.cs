using Common.Configurations;
using Common.Constants;
using PlayerBuildsService.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(PortsConstants.PlayerBuildsServiceHttpPort, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1;
    });

    options.ListenAnyIP(PortsConstants.PlayerBuildsServiceGrpcPort, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
    });
});

builder.Services.ConfigureGrpcServices();

var app = builder.Build();

app.MapGrpcEndpoints();

app.Run();
