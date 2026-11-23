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
            .OrderBy(x => x.Name)
            .ToListAsync();
    }
}
