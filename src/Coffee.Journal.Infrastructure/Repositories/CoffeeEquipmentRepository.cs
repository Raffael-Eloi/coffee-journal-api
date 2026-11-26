using Coffee.Journal.Domain.Contracts.Repositories;
using Coffee.Journal.Domain.Entities;
using Coffee.Journal.Infrastructure.Database.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Coffee.Journal.Infrastructure.Repositories;

internal sealed class CoffeeEquipmentRepository : BaseRepository<CoffeeEquipment>, ICoffeeEquipmentRepository
{
    public CoffeeEquipmentRepository(CoffeeJournalDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IReadOnlyList<CoffeeEquipment>> GetByProfileAsync(Guid profileId)
    {
        return await _dbContext.Set<CoffeeEquipment>()
            .AsNoTracking()
            .Where(x => x.ProfileId == profileId)
            .OrderBy(x => x.Name)
            .ToListAsync();
    }

    public override async Task<IReadOnlyList<CoffeeEquipment>> GetAllAsync()
    {
        return await _dbContext.Set<CoffeeEquipment>()
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync();
    }
}
