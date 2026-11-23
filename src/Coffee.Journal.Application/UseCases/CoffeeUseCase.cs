using Coffee.Journal.Application.Contracts.Models;
using Coffee.Journal.Application.Contracts.UseCases;
using Coffee.Journal.Application.Exceptions;
using Coffee.Journal.Application.Mappings;
using Coffee.Journal.Domain.Contracts.Repositories;
using Coffee.Journal.Domain.Entities;

namespace Coffee.Journal.Application.UseCases;

public sealed class CoffeeUseCase : ICoffeeUseCase
{
    private readonly ICoffeeRepository _repository;
    private readonly ICoffeeSupplierRepository _supplierRepository;

    public CoffeeUseCase(
        ICoffeeRepository repository,
        ICoffeeSupplierRepository supplierRepository)
    {
        _repository = repository;
        _supplierRepository = supplierRepository;
    }

    public async Task<Guid> AddAsync(CoffeeRequest request)
    {
        await EnsureSupplierExistsAsync(request.SupplierId.GetValueOrDefault());

        Domain.Entities.Coffee coffee = request.ToEntity();

        return await _repository.AddAsync(coffee);
    }

    public async Task<IReadOnlyList<CoffeeResponse>> GetAllAsync()
    {
        IReadOnlyList<Domain.Entities.Coffee> coffees = await _repository.GetAllAsync();

        return coffees.Select(x => x.ToResponse()).ToList();
    }

    public async Task<CoffeeResponse?> GetByIdAsync(Guid id)
    {
        Domain.Entities.Coffee? coffee = await _repository.GetByIdAsync(id);

        return coffee?.ToResponse();
    }

    public async Task<bool> UpdateAsync(Guid id, CoffeeRequest request)
    {
        Domain.Entities.Coffee? coffee = await _repository.GetByIdAsync(id);

        if (coffee is null)
        {
            return false;
        }

        await EnsureSupplierExistsAsync(request.SupplierId.GetValueOrDefault());

        request.ApplyTo(coffee);

        await _repository.UpdateAsync(coffee);

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _repository.DeleteAsync(id);
    }

    private async Task EnsureSupplierExistsAsync(Guid supplierId)
    {
        CoffeeSupplier? supplier = await _supplierRepository.GetByIdAsync(supplierId);

        if (supplier is null)
        {
            throw new RequestValidationException(
                "supplierId",
                $"Coffee supplier '{supplierId}' does not exist.");
        }
    }
}
