using Coffee.Journal.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Coffee.Journal.Infrastructure.Database.Contexts;

internal sealed class CoffeeJournalDbContext : DbContext
{
    public CoffeeJournalDbContext(DbContextOptions<CoffeeJournalDbContext> options)
        : base(options)
    {
    }

    public DbSet<CoffeeSupplier> CoffeeSuppliers { get; set; }

    public DbSet<CoffeeShop> CoffeeShops { get; set; }

    public DbSet<CoffeeShopImage> CoffeeShopImages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CoffeeJournalDbContext).Assembly);
    }
}
