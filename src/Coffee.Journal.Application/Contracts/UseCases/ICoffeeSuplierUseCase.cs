using Coffee.Journal.Application.Contracts.Models;

namespace Coffee.Journal.Application.Contracts.UseCases;

public interface ICoffeeSuplierUseCase
{
    Task<Guid> AddAsync(CoffeeSupplierRequest request);

    Task<IReadOnlyList<CoffeeSupplierResponse>> GetAllAsync();

    Task<CoffeeSupplierResponse?> GetByIdAsync(Guid id);

    Task<bool> UpdateAsync(Guid id, CoffeeSupplierRequest request);

    Task<bool> DeleteAsync(Guid id);
}
