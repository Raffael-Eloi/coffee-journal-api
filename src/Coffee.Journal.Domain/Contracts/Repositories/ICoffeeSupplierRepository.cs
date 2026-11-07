using Coffee.Journal.Domain.Entities;

namespace Coffee.Journal.Domain.Contracts.Repositories;

public interface ICoffeeSupplierRepository
{
    Task<Guid> AddAsync(CoffeeSupplier supplier);
}
