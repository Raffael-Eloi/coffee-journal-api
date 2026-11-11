using Coffee.Journal.Domain.Contracts.Repositories;
using Coffee.Journal.Domain.Entities;
using Coffee.Journal.Infrastructure.Database.Contexts;

namespace Coffee.Journal.Infrastructure.Repositories;

public class CoffeeSupplierRepository : ICoffeeSupplierRepository
{
    private readonly CoffeeJournalDbContext _dbContext;

    public CoffeeSupplierRepository(CoffeeJournalDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> AddAsync(CoffeeSupplier supplier)
    {
        supplier.CreatedAt = DateTimeOffset.UtcNow.DateTime;
        supplier.UpdatedAt = supplier.CreatedAt;

        await _dbContext.AddAsync(supplier);

        await _dbContext.SaveChangesAsync();

        return supplier.Id;
    }
}
