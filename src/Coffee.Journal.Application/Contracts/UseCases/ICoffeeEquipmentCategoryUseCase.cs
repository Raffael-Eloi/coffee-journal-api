using Coffee.Journal.Application.Contracts.Models;

namespace Coffee.Journal.Application.Contracts.UseCases;

public interface ICoffeeEquipmentCategoryUseCase
{
    Task<IReadOnlyList<CoffeeEquipmentCategoryResponse>> GetAllAsync();
}
