using Coffee.Journal.Domain.Entities;

namespace Coffee.Journal.Domain.Contracts.Repositories;

public interface ICoffeeSupplierRepository
{
    Task<Guid> AddAsync(CoffeeSupplier supplier);

    Task<IReadOnlyList<CoffeeSupplier>> GetAllAsync();

    Task<CoffeeSupplier?> GetByIdAsync(Guid id);

    Task UpdateAsync(CoffeeSupplier supplier);

    Task<bool> DeleteAsync(Guid id);
}
