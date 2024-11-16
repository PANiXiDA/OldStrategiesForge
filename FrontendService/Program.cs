using Tools.RabbitMQ.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(int.Parse(Environment.GetEnvironmentVariable("ASPNETCORE_HTTP_PORTS")!));
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
    name: "AdminDefaultRoute",
    pattern: "Admin/{controller=Home}/{action=Index}/{id?}",
    defaults: new { area = "Admin" });

app.MapControllerRoute(
    name: "AdminErrorRoute",
    pattern: "Admin/{controller=Error}/{code:int}",
    defaults: new { area = "Admin", action = "Index" });

app.MapControllerRoute(
    name: "PublicDefaultRoute",
    pattern: "{controller=Home}/{action=Index}/{id?}",
    defaults: new { area = "Public" });

app.MapControllerRoute(
    name: "PublicErrorRoute",
    pattern: "{controller=Error}/{code:int}",
    defaults: new { area = "Public", action = "Index" });

app.Run();
