using ProfileBackendService.Extensions;
using ProfileBackendService.Extensions.Mapping;

var builder = WebApplication.CreateBuilder(args);

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