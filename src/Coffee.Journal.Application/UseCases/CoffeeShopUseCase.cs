using Coffee.Journal.Application.Contracts.Models;
using Coffee.Journal.Application.Contracts.UseCases;
using Coffee.Journal.Application.Exceptions;
using Coffee.Journal.Application.Mappings;
using Coffee.Journal.Domain.Contracts.Repositories;
using Coffee.Journal.Domain.Entities;

namespace Coffee.Journal.Application.UseCases;

public sealed class CoffeeShopUseCase : ICoffeeShopUseCase
{
    private readonly ICoffeeShopRepository _repository;
    private readonly ICoffeeSupplierRepository _supplierRepository;

    public CoffeeShopUseCase(
        ICoffeeShopRepository repository,
        ICoffeeSupplierRepository supplierRepository)
    {
        _repository = repository;
        _supplierRepository = supplierRepository;
    }

    public async Task<Guid> AddAsync(CoffeeShopRequest request)
    {
        await EnsureSupplierExistsAsync(request.CoffeeSupplierId);

        CoffeeShop shop = request.ToEntity();

        return await _repository.AddAsync(shop);
    }

    public async Task<IReadOnlyList<CoffeeShopResponse>> GetAllAsync()
    {
        IReadOnlyList<CoffeeShop> shops = await _repository.GetAllAsync();

        return shops.Select(x => x.ToResponse()).ToList();
    }

    public async Task<CoffeeShopResponse?> GetByIdAsync(Guid id)
    {
        CoffeeShop? shop = await _repository.GetByIdAsync(id);

        return shop?.ToResponse();
    }

    public async Task<bool> UpdateAsync(Guid id, CoffeeShopRequest request)
    {
        CoffeeShop? shop = await _repository.GetByIdAsync(id);

        if (shop is null)
        {
            return false;
        }

        await EnsureSupplierExistsAsync(request.CoffeeSupplierId);

        request.ApplyTo(shop);

        await _repository.UpdateAsync(shop);

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _repository.DeleteAsync(id);
    }

    private async Task EnsureSupplierExistsAsync(Guid? supplierId)
    {
        if (supplierId is null)
        {
            return;
        }

        CoffeeSupplier? supplier = await _supplierRepository.GetByIdAsync(supplierId.Value);

        if (supplier is null)
        {
            throw new RequestValidationException(
                "coffeeSupplierId",
                $"Coffee supplier '{supplierId}' does not exist.");
        }
    }
}
