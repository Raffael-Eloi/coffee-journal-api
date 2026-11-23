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
    private readonly ICoffeeRoasterRepository _roasterRepository;

    public CoffeeShopUseCase(
        ICoffeeShopRepository repository,
        ICoffeeRoasterRepository roasterRepository)
    {
        _repository = repository;
        _roasterRepository = roasterRepository;
    }

    public async Task<Guid> AddAsync(CoffeeShopRequest request)
    {
        await EnsureRoasterExistsAsync(request.CoffeeRoasterId);

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

        await EnsureRoasterExistsAsync(request.CoffeeRoasterId);

        request.ApplyTo(shop);

        await _repository.UpdateAsync(shop);

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _repository.DeleteAsync(id);
    }

    private async Task EnsureRoasterExistsAsync(Guid? roasterId)
    {
        if (roasterId is null)
        {
            return;
        }

        CoffeeRoaster? roaster = await _roasterRepository.GetByIdAsync(roasterId.Value);

        if (roaster is null)
        {
            throw new RequestValidationException(
                "coffeeRoasterId",
                $"Coffee roaster '{roasterId}' does not exist.");
        }
    }
}
