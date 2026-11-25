using Coffee.Journal.Application.Contracts.Models;
using Coffee.Journal.Application.Contracts.UseCases;
using Coffee.Journal.Application.Mappings;
using Coffee.Journal.Domain.Contracts.Repositories;
using Coffee.Journal.Domain.Entities;

namespace Coffee.Journal.Application.UseCases;

public sealed class CoffeeEquipmentCategoryUseCase : ICoffeeEquipmentCategoryUseCase
{
    private readonly ICoffeeEquipmentCategoryRepository _repository;

    public CoffeeEquipmentCategoryUseCase(ICoffeeEquipmentCategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<CoffeeEquipmentCategoryResponse>> GetAllAsync()
    {
        IReadOnlyList<CoffeeEquipmentCategory> categories = await _repository.GetAllAsync();

        return categories.Select(x => x.ToResponse()).ToList();
    }
}
