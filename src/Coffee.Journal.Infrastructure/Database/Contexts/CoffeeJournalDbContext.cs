using Coffee.Journal.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Coffee.Journal.Infrastructure.Database.Contexts;

internal sealed class CoffeeJournalDbContext : DbContext
{
    public CoffeeJournalDbContext(DbContextOptions<CoffeeJournalDbContext> options)
        : base(options)
    {
    }

    public DbSet<CoffeeRoaster> CoffeeRoasters { get; set; }

    public DbSet<CoffeeShop> CoffeeShops { get; set; }

    public DbSet<CoffeeShopImage> CoffeeShopImages { get; set; }

    public DbSet<Domain.Entities.Coffee> Coffees { get; set; }

    public DbSet<CoffeePrice> CoffeePrices { get; set; }

    public DbSet<CoffeeImage> CoffeeImages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CoffeeJournalDbContext).Assembly);
    }
}
