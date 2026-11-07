using Coffee.Journal.Application.Contracts.Models;

namespace Coffee.Journal.Application.Contracts.UseCases;

public interface ICoffeeSuplierUseCase
{
    Task<Guid> AddAsync(CoffeeSupplierRequest request);
}
