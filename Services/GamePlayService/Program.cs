using Common.Configurations;
using GamePlayService.Extensions;
using GamePlayService.Extensions.Helpers;
using GamePlayService.Services;
using Microsoft.Extensions.Options;
using GameEngineDotnetDI;
using GamePlayService.BL.BL.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<JwtSettings>>().Value);
builder.Services.AddSingleton<JwtHelper>();

builder.Services.ConfigureGrpcClients();
builder.Services.ResolveDependencyInjection();
builder.Services.AddBusinessLogicLayer();

builder.Services.AddHostedService<GamePlayServiceImpl>();

var app = builder.Build();

app.Run();