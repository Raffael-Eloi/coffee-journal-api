using Coffee.Journal.Infrastructure;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace Coffee.Journal.API.Endpoints;

public sealed class HealthEndpoint
{
    public static void Map(WebApplication app)
    {
        // Liveness: the process is up. It runs no checks, so it never fails because of a dependency.
        app.MapHealthChecks("/health/live", new HealthCheckOptions { Predicate = _ => false })
            .AllowAnonymous();

        // Readiness: the app can serve requests, so it also checks the database.
        app.MapHealthChecks("/health/ready", new HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains(DependencyInjection.ReadyTag)
        })
            .AllowAnonymous();
    }
}
