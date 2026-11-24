using Coffee.Journal.Application.Contracts.Models;
using Coffee.Journal.Application.Contracts.UseCases;
using Coffee.Journal.Application.Exceptions;
using Coffee.Journal.Application.Mappings;
using Coffee.Journal.Domain.Contracts.Repositories;
using Coffee.Journal.Domain.Contracts.Storage;
using Coffee.Journal.Domain.Entities;

namespace Coffee.Journal.Application.UseCases;

public sealed class CoffeeUseCase : ICoffeeUseCase
{
    private readonly ICoffeeRepository _repository;
    private readonly ICoffeeRoasterRepository _roasterRepository;
    private readonly IImageStorage _imageStorage;

    public CoffeeUseCase(
        ICoffeeRepository repository,
        ICoffeeRoasterRepository roasterRepository,
        IImageStorage imageStorage)
    {
        _repository = repository;
        _roasterRepository = roasterRepository;
        _imageStorage = imageStorage;
    }

    public async Task<Guid> AddAsync(CoffeeRequest request)
    {
        await EnsureRoasterExistsAsync(request.RoasterId.GetValueOrDefault());
        EnsureImagesBelongToStorage(request);

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

        await EnsureRoasterExistsAsync(request.RoasterId.GetValueOrDefault());
        EnsureImagesBelongToStorage(request);

        List<string> previousUrls = coffee.Images.Select(x => x.Url).ToList();

        request.ApplyTo(coffee);

        await _repository.UpdateAsync(coffee);

        foreach (string removedUrl in previousUrls.Except(request.ImageUrls))
        {
            await _imageStorage.DeleteAsync(removedUrl);
        }

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        Domain.Entities.Coffee? coffee = await _repository.GetByIdAsync(id);

        if (coffee is null)
        {
            return false;
        }

        List<string> urls = coffee.Images.Select(x => x.Url).ToList();

        bool deleted = await _repository.DeleteAsync(id);

        if (deleted)
        {
            foreach (string url in urls)
            {
                await _imageStorage.DeleteAsync(url);
            }
        }

        return deleted;
    }

    private void EnsureImagesBelongToStorage(CoffeeRequest request)
    {
        string? invalidUrl = request.ImageUrls.FirstOrDefault(url => !_imageStorage.IsStorageUrl(url));

        if (invalidUrl is not null)
        {
            throw new RequestValidationException(
                "imageUrls",
                $"Image '{invalidUrl}' was not uploaded through the images endpoint.");
        }
    }

    private async Task EnsureRoasterExistsAsync(Guid roasterId)
    {
        CoffeeRoaster? roaster = await _roasterRepository.GetByIdAsync(roasterId);

        if (roaster is null)
        {
            throw new RequestValidationException(
                "roasterId",
                $"Coffee roaster '{roasterId}' does not exist.");
        }
    }
}
