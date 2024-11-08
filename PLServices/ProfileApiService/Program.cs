using Players.Backend.Gen;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddGrpcClient<PlayersBackend.PlayersBackendClient> (o =>
{
    var url = Environment.GetEnvironmentVariable("PlayersBackendUrl");
    if (url != null)
    {
        o.Address = new Uri(url);
    }
    else
    {
        throw new InvalidOperationException("PlayersBackendUrl is not set.");
    }
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
