using PlayersBackendService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<PlayersApiServiceImpl>();
app.MapGet("/", () => "gRPC server is running");

app.Run();
