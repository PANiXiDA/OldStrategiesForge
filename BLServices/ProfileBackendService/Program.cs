using ProfileBackendService.Extensions;
using ProfileBackendService.Extensions.Mapping;

var builder = WebApplication.CreateBuilder(args);

PortsConfiguration.ConfigurePort();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(PortsConfiguration.HttpPort, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1;
    });

    options.ListenAnyIP(PortsConfiguration.GrpcPort, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
    });
});

builder.Services.ConfigureGrpcClients();
builder.Services.ConfigureGrpcServices();

builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

app.MapGrpcEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapGrpcReflectionService();
}

app.Run();