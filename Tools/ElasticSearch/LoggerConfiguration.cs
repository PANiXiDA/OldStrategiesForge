using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.OpenSearch;

namespace Tools.ElasticSearch;

public static class LoggerConfiguration
{
    public static ILogger ConfigureLogger(string indexPrefix, IConfiguration configuration)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        var serilog = new Serilog.LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .Enrich.WithEnvironmentName()
            .Enrich.WithMachineName()
            .WriteTo.Console()
            .WriteTo.Debug()
            .WriteTo.OpenSearch(new OpenSearchSinkOptions(new Uri(configuration["ElasticSearch:Url"]!))
            {
                AutoRegisterTemplate = true,
                AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.OSv1,
                IndexFormat = $"{indexPrefix}-{environment?.ToLower().Replace(".", "-")}",
                ModifyConnectionSettings = x => x
                    .BasicAuthentication(configuration["ElasticSearch:Login"], configuration["ElasticSearch:Password"])
                    .ThrowExceptions(true),
            })
            .ReadFrom.Configuration(configuration)
            .CreateLogger();
        return serilog;
    }
}
