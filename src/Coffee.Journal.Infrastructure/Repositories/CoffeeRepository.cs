using Coffee.Journal.Domain.Contracts.Repositories;
using Coffee.Journal.Domain.Entities;
using Coffee.Journal.Infrastructure.Database.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Coffee.Journal.Infrastructure.Repositories;

internal sealed class CoffeeRepository : BaseRepository<Domain.Entities.Coffee>, ICoffeeRepository
{
    public CoffeeRepository(CoffeeJournalDbContext dbContext) : base(dbContext)
    {
    }

    public override async Task<IReadOnlyList<Domain.Entities.Coffee>> GetAllAsync()
    {
        return await _dbContext.Set<Domain.Entities.Coffee>()
            .AsNoTracking()
            .Include(x => x.Prices)
            .Include(x => x.Images)
            .AsSplitQuery()
            .OrderBy(x => x.Name)
            .ToListAsync();
    }

    public override async Task<Domain.Entities.Coffee?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Set<Domain.Entities.Coffee>()
            .Include(x => x.Prices)
            .Include(x => x.Images)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}
