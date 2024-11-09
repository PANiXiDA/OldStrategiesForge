using ProfileDatabaseService.DAL.Implementations.Extensions;
using ProfileDatabaseService.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.ConfigureGrpcServices();

var app = builder.Build();

app.MapGrpcEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapGrpcReflectionService();
}

app.Run();
