using Coffee.Journal.Domain.Entities;

namespace Coffee.Journal.Domain.Contracts.Repositories;

public interface ICoffeeEquipmentRepository : IBaseRepository<CoffeeEquipment>
{
    Task<IReadOnlyList<CoffeeEquipment>> GetByProfileAsync(Guid profileId);
}
