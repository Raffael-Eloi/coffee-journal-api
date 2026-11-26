using Coffee.Journal.Domain.Entities;

namespace Coffee.Journal.Domain.Contracts.Repositories;

public interface IProfileRepository : IBaseRepository<Profile>
{
    Task<Profile?> GetByExternalIdAsync(string externalId);

    Task<bool> ExternalIdExistsAsync(string externalId, Guid? excludeId = null);

    Task<bool> EmailExistsAsync(string email, Guid? excludeId = null);

    Task<bool> HandleExistsAsync(string handle, Guid? excludeId = null);
}
