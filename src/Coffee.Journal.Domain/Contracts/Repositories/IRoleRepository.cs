using Coffee.Journal.Domain.Entities;

namespace Coffee.Journal.Domain.Contracts.Repositories;

public interface IRoleRepository : IBaseRepository<Role>
{
    Task<Role?> GetByNameAsync(string name);
}
