using PlayersService.DAL.SQL.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddGrpc();

var app = builder.Build();

app.Run();
