using Coffee.Journal.Infrastructure.Database.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Coffee.Journal.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        string connectionString)
    {
        services.AddDbContext<CoffeeJournalDbContext>(options =>
            options.UseSqlServer(connectionString));

        return services;
    }
}
