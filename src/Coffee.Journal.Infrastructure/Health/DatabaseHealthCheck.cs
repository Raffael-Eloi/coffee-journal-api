using Coffee.Journal.Infrastructure.Database.Contexts;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Coffee.Journal.Infrastructure.Health;

/// <summary>Reports healthy when the app can open a connection to the database.</summary>
internal sealed class DatabaseHealthCheck(CoffeeJournalDbContext dbContext) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        bool canConnect = await dbContext.Database.CanConnectAsync(cancellationToken);

        return canConnect
            ? HealthCheckResult.Healthy()
            : HealthCheckResult.Unhealthy("Cannot connect to the database.");
    }
}
