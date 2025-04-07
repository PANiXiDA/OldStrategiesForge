using Common.Constants;
using Tools.RabbitMQ.Extensions;
using Serilog;
using LoggerConfiguration = Tools.ElasticSearch.LoggerConfiguration;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(LoggerConfiguration.ConfigureLogger(ServiceNames.FrontendService, builder.Configuration));

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(PortsConstants.FrontendServiceHttpPort);
});

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
builder.Services.AddMessageBrokers(builder.Configuration, environment);

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "PublicDefaultRoute",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "PublicErrorRoute",
    pattern: "{controller=Error}/{code:int}",
    defaults: new { action = "Index" });

app.Run();
