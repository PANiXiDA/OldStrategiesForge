using ProfileDatabaseService.DAL.Implementations.Extensions;
using ProfileDatabaseService.Services;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

var app = builder.Build();

app.MapGrpcService<PlayersDatabaseServiceImpl>();

if (app.Environment.IsDevelopment())
{
    app.MapGrpcReflectionService();
}

app.Run();
