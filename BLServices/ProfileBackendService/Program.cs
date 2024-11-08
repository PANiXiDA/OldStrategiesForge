using Players.Database.Gen;
using ProfileBackendService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

builder.Services.AddGrpcClient<PlayersDatabase.PlayersDatabaseClient>(o =>
{
    var url = Environment.GetEnvironmentVariable("PlayersDatabaseUrl");
    if (url != null)
    {
        o.Address = new Uri(url);
    }
    else
    {
        throw new InvalidOperationException("PlayersDatabaseUrl is not set.");
    }
});

var app = builder.Build();

app.MapGrpcService<PlayersBackendServiceImpl>();

if (app.Environment.IsDevelopment())
{
    app.MapGrpcReflectionService();
}

app.Run();
