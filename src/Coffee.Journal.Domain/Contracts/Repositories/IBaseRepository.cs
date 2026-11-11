using Coffee.Journal.Domain.Entities;

namespace Coffee.Journal.Domain.Contracts.Repositories;

public interface IBaseRepository<TEntity> where TEntity : BaseEntity 
{
    Task<Guid> AddAsync(TEntity entity);

    Task<IReadOnlyList<TEntity>> GetAllAsync();

    Task<TEntity?> GetByIdAsync(Guid id);

    Task UpdateAsync(TEntity entity);

    Task<bool> DeleteAsync(Guid id);
}
