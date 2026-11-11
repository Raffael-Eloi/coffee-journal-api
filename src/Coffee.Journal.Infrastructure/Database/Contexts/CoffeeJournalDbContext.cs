using Coffee.Journal.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Coffee.Journal.Infrastructure.Database.Contexts;

// TODO: should this class be public?
public class CoffeeJournalDbContext : DbContext
{
    public CoffeeJournalDbContext(DbContextOptions<CoffeeJournalDbContext> options)
        : base(options)
    {
    }

    public DbSet<CoffeeSupplier> CoffeeSuppliers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CoffeeJournalDbContext).Assembly);
    }
}
