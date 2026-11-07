using Coffee.Journal.Application.Contracts.Models;
using Coffee.Journal.Application.Contracts.UseCases;
using Coffee.Journal.Domain.Contracts.Repositories;
using Coffee.Journal.Domain.Entities;

namespace Coffee.Journal.Application.UseCases;

public class CoffeeSuplierUseCase : ICoffeeSuplierUseCase
{
    private readonly ICoffeeSupplierRepository _repository;

    public CoffeeSuplierUseCase(ICoffeeSupplierRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> AddAsync(CoffeeSupplierRequest request)
    {
        CoffeeSupplier supplier = new()
        {
            Name = request.Name,
            Website = request.Website,
            Recommend = request.Recommend.GetValueOrDefault(),
            HasMembershipOption = request.HasMembershipOption.GetValueOrDefault(),
            Rate = request.Rate,
            Review = request.Review,
            LogoUrl = request.LogoUrl,
        };

        Guid supplierId = await _repository.AddAsync(supplier);
        return supplierId;
    }
}
