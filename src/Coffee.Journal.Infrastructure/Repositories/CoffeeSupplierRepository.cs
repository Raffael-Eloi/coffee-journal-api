using Coffee.Journal.Domain.Contracts.Repositories;
using Coffee.Journal.Domain.Entities;
using Coffee.Journal.Infrastructure.Database.Contexts;
using Microsoft.EntityFrameworkCore;

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

    public async Task<IReadOnlyList<CoffeeSupplier>> GetAllAsync()
    {
        return await _dbContext.CoffeeSuppliers
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync();
    }

    public async Task<CoffeeSupplier?> GetByIdAsync(Guid id)
    {
        return await _dbContext.CoffeeSuppliers.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task UpdateAsync(CoffeeSupplier supplier)
    {
        supplier.UpdatedAt = DateTimeOffset.UtcNow.DateTime;

        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        int affectedRows = await _dbContext.CoffeeSuppliers
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync();

        return affectedRows > 0;
    }
}
