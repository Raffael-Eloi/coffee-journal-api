using Coffee.Journal.Application.Contracts.Models;
using Coffee.Journal.Application.Contracts.UseCases;
using Coffee.Journal.Application.Exceptions;
using Coffee.Journal.Application.Mappings;
using Coffee.Journal.Domain.Contracts.Repositories;
using Coffee.Journal.Domain.Contracts.Storage;
using Coffee.Journal.Domain.Entities;

namespace Coffee.Journal.Application.UseCases;

public sealed class CoffeeRoasterUseCase : ICoffeeRoasterUseCase
{
    private readonly ICoffeeRoasterRepository _repository;
    private readonly IImageStorage _imageStorage;

    public CoffeeRoasterUseCase(ICoffeeRoasterRepository repository, IImageStorage imageStorage)
    {
        _repository = repository;
        _imageStorage = imageStorage;
    }

    public async Task<Guid> AddAsync(CoffeeRoasterRequest request)
    {
        EnsureImagesBelongToStorage(request);

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

        EnsureImagesBelongToStorage(request);

        List<string> previousUrls = roaster.Images.Select(x => x.Url).ToList();

        request.ApplyTo(roaster);

        await _repository.UpdateAsync(roaster);

        foreach (string removedUrl in previousUrls.Except(request.ImageUrls))
        {
            await _imageStorage.DeleteAsync(removedUrl);
        }

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        CoffeeRoaster? roaster = await _repository.GetByIdAsync(id);

        if (roaster is null)
        {
            return false;
        }

        List<string> urls = roaster.Images.Select(x => x.Url).ToList();

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

    private void EnsureImagesBelongToStorage(CoffeeRoasterRequest request)
    {
        string? invalidUrl = request.ImageUrls.FirstOrDefault(url => !_imageStorage.IsStorageUrl(url));

        if (invalidUrl is not null)
        {
            throw new RequestValidationException(
                "imageUrls",
                $"Image '{invalidUrl}' was not uploaded through the images endpoint.");
        }
    }
}
