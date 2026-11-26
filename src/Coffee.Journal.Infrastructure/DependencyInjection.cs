using Azure.Storage.Blobs;
using Coffee.Journal.Domain.Contracts.Repositories;
using Coffee.Journal.Domain.Contracts.Storage;
using Coffee.Journal.Infrastructure.Database.Contexts;
using Coffee.Journal.Infrastructure.Repositories;
using Coffee.Journal.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Coffee.Journal.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        string connectionString,
        BlobStorageOptions blobStorageOptions)
    {
        services.AddDbContext<CoffeeJournalDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IProfileRepository, ProfileRepository>();
        services.AddScoped<ICoffeeRoasterRepository, CoffeeRoasterRepository>();
        services.AddScoped<ICoffeeShopRepository, CoffeeShopRepository>();
        services.AddScoped<ICoffeeRepository, CoffeeRepository>();
        services.AddScoped<ICoffeeEquipmentRepository, CoffeeEquipmentRepository>();
        services.AddScoped<ICoffeeEquipmentCategoryRepository, CoffeeEquipmentCategoryRepository>();

        services.AddSingleton<IImageStorage>(serviceProvider => new AzureBlobImageStorage(
            new BlobContainerClient(blobStorageOptions.ConnectionString, blobStorageOptions.ContainerName),
            serviceProvider.GetRequiredService<ILogger<AzureBlobImageStorage>>()));

        return services;
    }

    public static void ApplyMigrations(this IServiceProvider serviceProvider)
    {
        using IServiceScope scope = serviceProvider.CreateScope();
        scope.ServiceProvider
            .GetRequiredService<CoffeeJournalDbContext>()
            .Database
            .Migrate();
    }
}
