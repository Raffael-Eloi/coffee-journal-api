using Coffee.Journal.Application.Contracts.Models;
using Coffee.Journal.Application.Contracts.UseCases;
using Coffee.Journal.Application.Mappings;
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

        return await _repository.AddAsync(supplier);
    }

    public async Task<IReadOnlyList<CoffeeSupplierResponse>> GetAllAsync()
    {
        IReadOnlyList<CoffeeSupplier> suppliers = await _repository.GetAllAsync();

        return suppliers.Select(x => x.ToResponse()).ToList();
    }

    public async Task<CoffeeSupplierResponse?> GetByIdAsync(Guid id)
    {
        CoffeeSupplier? supplier = await _repository.GetByIdAsync(id);

        return supplier?.ToResponse();
    }

    public async Task<bool> UpdateAsync(Guid id, CoffeeSupplierRequest request)
    {
        CoffeeSupplier? supplier = await _repository.GetByIdAsync(id);

        if (supplier is null)
        {
            return false;
        }

        supplier.Name = request.Name;
        supplier.Website = request.Website;
        supplier.Recommend = request.Recommend.GetValueOrDefault();
        supplier.HasMembershipOption = request.HasMembershipOption.GetValueOrDefault();
        supplier.Rate = request.Rate;
        supplier.Review = request.Review;
        supplier.LogoUrl = request.LogoUrl;

        await _repository.UpdateAsync(supplier);

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _repository.DeleteAsync(id);
    }
}
