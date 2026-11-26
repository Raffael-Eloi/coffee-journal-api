using Coffee.Journal.Application.Contracts;
using Coffee.Journal.Application.Contracts.Models;
using Coffee.Journal.Application.Contracts.UseCases;
using Coffee.Journal.Application.Exceptions;
using Coffee.Journal.Application.Mappings;
using Coffee.Journal.Domain.Contracts.Repositories;
using Coffee.Journal.Domain.Contracts.Storage;
using Coffee.Journal.Domain.Entities;

namespace Coffee.Journal.Application.UseCases;

public sealed class CoffeeEquipmentUseCase : ICoffeeEquipmentUseCase
{
    private readonly ICoffeeEquipmentRepository _repository;
    private readonly ICoffeeEquipmentCategoryRepository _categoryRepository;
    private readonly IProfileRepository _profileRepository;
    private readonly IImageStorage _imageStorage;
    private readonly ICurrentUser _currentUser;

    public CoffeeEquipmentUseCase(
        ICoffeeEquipmentRepository repository,
        ICoffeeEquipmentCategoryRepository categoryRepository,
        IProfileRepository profileRepository,
        IImageStorage imageStorage,
        ICurrentUser currentUser)
    {
        _repository = repository;
        _categoryRepository = categoryRepository;
        _profileRepository = profileRepository;
        _imageStorage = imageStorage;
        _currentUser = currentUser;
    }

    public async Task<Guid> AddAsync(CoffeeEquipmentRequest request)
    {
        Profile profile = await GetCurrentProfileAsync();
        await EnsureCategoryExistsAsync(request.EquipmentCategoryId.GetValueOrDefault());
        EnsureImageBelongsToStorage(request);

        return await _repository.AddAsync(request.ToEntity(profile.Id));
    }

    public async Task<IReadOnlyList<CoffeeEquipmentResponse>> GetAllAsync()
    {
        IReadOnlyList<CoffeeEquipment> equipments = await _repository.GetAllAsync();

        return equipments.Select(x => x.ToResponse()).ToList();
    }

    public async Task<CoffeeEquipmentResponse?> GetByIdAsync(Guid id)
    {
        CoffeeEquipment? equipment = await _repository.GetByIdAsync(id);

        return equipment?.ToResponse();
    }

    public async Task<bool> UpdateAsync(Guid id, CoffeeEquipmentRequest request)
    {
        CoffeeEquipment? equipment = await _repository.GetByIdAsync(id);

        if (equipment is null)
        {
            return false;
        }

        await EnsureCanChangeAsync(equipment);
        await EnsureCategoryExistsAsync(request.EquipmentCategoryId.GetValueOrDefault());
        EnsureImageBelongsToStorage(request);

        string? previousUrl = equipment.ImageUrl;

        request.ApplyTo(equipment);

        await _repository.UpdateAsync(equipment);

        if (previousUrl is not null && previousUrl != request.ImageUrl)
        {
            await _imageStorage.DeleteAsync(previousUrl);
        }

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        CoffeeEquipment? equipment = await _repository.GetByIdAsync(id);

        if (equipment is null)
        {
            return false;
        }

        await EnsureCanChangeAsync(equipment);

        string? url = equipment.ImageUrl;

        bool deleted = await _repository.DeleteAsync(id);

        if (deleted && url is not null)
        {
            await _imageStorage.DeleteAsync(url);
        }

        return deleted;
    }

    private void EnsureImageBelongsToStorage(CoffeeEquipmentRequest request)
    {
        if (request.ImageUrl is not null && !_imageStorage.IsStorageUrl(request.ImageUrl))
        {
            throw new RequestValidationException(
                "imageUrl",
                $"Image '{request.ImageUrl}' was not uploaded through the images endpoint.");
        }
    }

    private async Task<Profile> GetCurrentProfileAsync()
    {
        string externalId = _currentUser.ExternalId
            ?? throw new InvalidOperationException("There is no authenticated user for this request.");

        return await _profileRepository.GetByExternalIdAsync(externalId)
            ?? throw new RequestValidationException("profile", "Create your profile before adding equipment.");
    }

    /// <summary>Only the owner or an admin can change a piece of equipment.</summary>
    private async Task EnsureCanChangeAsync(CoffeeEquipment equipment)
    {
        Profile profile = await GetCurrentProfileAsync();

        bool isAdmin = profile.Roles.Any(x => x.Role?.Name == RoleNames.Admin);

        if (equipment.ProfileId != profile.Id && !isAdmin)
        {
            throw new ForbiddenException("This equipment belongs to another profile.");
        }
    }

    private async Task EnsureCategoryExistsAsync(Guid categoryId)
    {
        CoffeeEquipmentCategory? category = await _categoryRepository.GetByIdAsync(categoryId);

        if (category is null)
        {
            throw new RequestValidationException(
                "equipmentCategoryId",
                $"Coffee equipment category '{categoryId}' does not exist.");
        }
    }
}
