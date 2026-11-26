using Coffee.Journal.Domain.Contracts.Repositories;
using Coffee.Journal.Domain.Entities;
using Coffee.Journal.Infrastructure.Database.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Coffee.Journal.Infrastructure.Repositories;

internal sealed class CoffeeRoasterRepository : BaseRepository<CoffeeRoaster>, ICoffeeRoasterRepository
{
    public CoffeeRoasterRepository(CoffeeJournalDbContext dbContext) : base(dbContext)
    {
    }

    public override async Task<IReadOnlyList<CoffeeRoaster>> GetAllAsync()
    {
        return await _dbContext.Set<CoffeeRoaster>()
            .AsNoTracking()
            .Include(x => x.Images)
            .Include(x => x.Classifications)
            .AsSplitQuery()
            .OrderBy(x => x.Name)
            .ToListAsync();
    }

    public override async Task<CoffeeRoaster?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Set<CoffeeRoaster>()
            .Include(x => x.Images)
            .Include(x => x.Classifications)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}
