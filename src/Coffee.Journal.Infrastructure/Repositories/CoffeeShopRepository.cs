using Coffee.Journal.Domain.Contracts.Repositories;
using Coffee.Journal.Domain.Entities;
using Coffee.Journal.Infrastructure.Database.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Coffee.Journal.Infrastructure.Repositories;

internal sealed class CoffeeShopRepository : BaseRepository<CoffeeShop>, ICoffeeShopRepository
{
    public CoffeeShopRepository(CoffeeJournalDbContext dbContext) : base(dbContext)
    {
    }

    public override async Task<IReadOnlyList<CoffeeShop>> GetAllAsync()
    {
        return await _dbContext.Set<CoffeeShop>()
            .AsNoTracking()
            .Include(x => x.Images)
            .OrderBy(x => x.Name)
            .ToListAsync();
    }

    public override async Task<CoffeeShop?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Set<CoffeeShop>()
            .Include(x => x.Images)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}
