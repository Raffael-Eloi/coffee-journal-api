using Coffee.Journal.Domain.Contracts.Repositories;
using Coffee.Journal.Domain.Entities;
using Coffee.Journal.Infrastructure.Database.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Coffee.Journal.Infrastructure.Repositories;

internal sealed class ProfileRepository : BaseRepository<Profile>, IProfileRepository
{
    public ProfileRepository(CoffeeJournalDbContext dbContext) : base(dbContext)
    {
    }

    public override async Task<IReadOnlyList<Profile>> GetAllAsync()
    {
        return await _dbContext.Set<Profile>()
            .AsNoTracking()
            .Include(x => x.Roles)
            .ThenInclude(x => x.Role)
            .OrderBy(x => x.Handle)
            .ToListAsync();
    }

    public override async Task<Profile?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Set<Profile>()
            .Include(x => x.Roles)
            .ThenInclude(x => x.Role)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> ExternalIdExistsAsync(string externalId, Guid? excludeId = null)
    {
        return await _dbContext.Set<Profile>()
            .AnyAsync(x => x.ExternalId == externalId && x.Id != excludeId);
    }

    public async Task<bool> EmailExistsAsync(string email, Guid? excludeId = null)
    {
        return await _dbContext.Set<Profile>()
            .AnyAsync(x => x.Email == email && x.Id != excludeId);
    }

    public async Task<bool> HandleExistsAsync(string handle, Guid? excludeId = null)
    {
        return await _dbContext.Set<Profile>()
            .AnyAsync(x => x.Handle == handle && x.Id != excludeId);
    }
}
