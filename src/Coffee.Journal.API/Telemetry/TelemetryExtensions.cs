using Azure.Monitor.OpenTelemetry.AspNetCore;

namespace Coffee.Journal.API.Telemetry;

internal static class TelemetryExtensions
{
    /// <summary>Environment variable (or setting) that the Azure Monitor exporter reads.</summary>
    private const string ConnectionStringKey = "APPLICATIONINSIGHTS_CONNECTION_STRING";

    /// <summary>
    /// Sends logs, traces and metrics to Application Insights through OpenTelemetry.
    /// It stays off when there is no connection string, so local development needs no setup.
    /// </summary>
    public static IServiceCollection AddApiTelemetry(this IServiceCollection services, IConfiguration configuration)
    {
        if (string.IsNullOrWhiteSpace(configuration[ConnectionStringKey]))
        {
            return services;
        }

        services.AddOpenTelemetry().UseAzureMonitor();

        return services;
    }
}
