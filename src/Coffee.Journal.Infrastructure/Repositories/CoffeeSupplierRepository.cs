using Coffee.Journal.Domain.Contracts.Repositories;
using Coffee.Journal.Domain.Entities;
using Coffee.Journal.Infrastructure.Database.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Coffee.Journal.Infrastructure.Repositories;

internal sealed class CoffeeSupplierRepository : BaseRepository<CoffeeSupplier>, ICoffeeSupplierRepository
{
    private readonly CoffeeJournalDbContext _dbContext;

    public CoffeeSupplierRepository(CoffeeJournalDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public override async Task<IReadOnlyList<CoffeeSupplier>> GetAllAsync()
    {
        return await _dbContext.Set<CoffeeSupplier>()
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync();
    }
}
