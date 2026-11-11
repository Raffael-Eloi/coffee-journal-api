using Coffee.Journal.Domain.Contracts.Repositories;
using Coffee.Journal.Domain.Entities;
using Coffee.Journal.Infrastructure.Database.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Coffee.Journal.Infrastructure.Repositories;

internal abstract class BaseRepository<TEntity> : IBaseRepository<TEntity>
    where TEntity : BaseEntity
{
    private readonly CoffeeJournalDbContext _dbContext;

    protected BaseRepository(CoffeeJournalDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> AddAsync(TEntity entity)
    {
        entity.CreatedAt = DateTimeOffset.UtcNow.DateTime;
        entity.UpdatedAt = entity.CreatedAt;

        await _dbContext.Set<TEntity>().AddAsync(entity);

        await _dbContext.SaveChangesAsync();

        return entity.Id;
    }

    public virtual async Task<IReadOnlyList<TEntity>> GetAllAsync()
    {
        return await _dbContext.Set<TEntity>()
            .AsNoTracking()
            .OrderBy(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task<TEntity?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task UpdateAsync(TEntity entity)
    {
        entity.UpdatedAt = DateTimeOffset.UtcNow.DateTime;

        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        int affectedRows = await _dbContext.Set<TEntity>()
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync();

        return affectedRows > 0;
    }
}
