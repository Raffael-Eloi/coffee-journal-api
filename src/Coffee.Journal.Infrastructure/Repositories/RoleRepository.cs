using Coffee.Journal.Domain.Contracts.Repositories;
using Coffee.Journal.Domain.Entities;
using Coffee.Journal.Infrastructure.Database.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Coffee.Journal.Infrastructure.Repositories;

internal sealed class RoleRepository : BaseRepository<Role>, IRoleRepository
{
    public RoleRepository(CoffeeJournalDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Role?> GetByNameAsync(string name)
    {
        return await _dbContext.Set<Role>()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name == name);
    }
}
