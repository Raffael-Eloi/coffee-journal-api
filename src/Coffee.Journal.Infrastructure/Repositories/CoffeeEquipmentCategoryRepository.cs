using Coffee.Journal.Domain.Contracts.Repositories;
using Coffee.Journal.Domain.Entities;
using Coffee.Journal.Infrastructure.Database.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Coffee.Journal.Infrastructure.Repositories;

internal sealed class CoffeeEquipmentCategoryRepository : BaseRepository<CoffeeEquipmentCategory>, ICoffeeEquipmentCategoryRepository
{
    public CoffeeEquipmentCategoryRepository(CoffeeJournalDbContext dbContext) : base(dbContext)
    {
    }

    public override async Task<IReadOnlyList<CoffeeEquipmentCategory>> GetAllAsync()
    {
        return await _dbContext.Set<CoffeeEquipmentCategory>()
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync();
    }
}
