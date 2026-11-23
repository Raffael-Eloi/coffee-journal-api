using Coffee.Journal.Application.Contracts.Models;
using Coffee.Journal.Application.Contracts.UseCases;
using Coffee.Journal.Application.Mappings;
using Coffee.Journal.Domain.Contracts.Repositories;
using Coffee.Journal.Domain.Entities;

namespace Coffee.Journal.Application.UseCases;

public sealed class CoffeeRoasterUseCase : ICoffeeRoasterUseCase
{
    private readonly ICoffeeRoasterRepository _repository;

    public CoffeeRoasterUseCase(ICoffeeRoasterRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> AddAsync(CoffeeRoasterRequest request)
    {
        CoffeeRoaster roaster = request.ToEntity();

        return await _repository.AddAsync(roaster);
    }

    public async Task<IReadOnlyList<CoffeeRoasterResponse>> GetAllAsync()
    {
        IReadOnlyList<CoffeeRoaster> roasters = await _repository.GetAllAsync();

        return roasters.Select(x => x.ToResponse()).ToList();
    }

    public async Task<CoffeeRoasterResponse?> GetByIdAsync(Guid id)
    {
        CoffeeRoaster? roaster = await _repository.GetByIdAsync(id);

        return roaster?.ToResponse();
    }

    public async Task<bool> UpdateAsync(Guid id, CoffeeRoasterRequest request)
    {
        CoffeeRoaster? roaster = await _repository.GetByIdAsync(id);

        if (roaster is null)
        {
            return false;
        }

        request.ApplyTo(roaster);

        await _repository.UpdateAsync(roaster);

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _repository.DeleteAsync(id);
    }
}
