using Coffee.Journal.Application.Contracts.Models;
using Coffee.Journal.Application.Contracts.UseCases;
using Coffee.Journal.Application.Exceptions;
using Coffee.Journal.Application.Mappings;
using Coffee.Journal.Domain.Contracts.Repositories;
using Coffee.Journal.Domain.Entities;

namespace Coffee.Journal.Application.UseCases;

public sealed class ProfileUseCase : IProfileUseCase
{
    private readonly IProfileRepository _repository;

    public ProfileUseCase(IProfileRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> AddAsync(ProfileRequest request)
    {
        if (await _repository.ExternalIdExistsAsync(request.ExternalId))
        {
            throw new RequestValidationException("externalId", "A profile for this user already exists.");
        }

        await EnsureUniqueAsync(request, excludeId: null);

        return await _repository.AddAsync(request.ToEntity());
    }

    public async Task<IReadOnlyList<ProfileResponse>> GetAllAsync()
    {
        IReadOnlyList<Profile> profiles = await _repository.GetAllAsync();

        return profiles.Select(x => x.ToResponse()).ToList();
    }

    public async Task<ProfileResponse?> GetByIdAsync(Guid id)
    {
        Profile? profile = await _repository.GetByIdAsync(id);

        return profile?.ToResponse();
    }

    public async Task<bool> UpdateAsync(Guid id, ProfileRequest request)
    {
        Profile? profile = await _repository.GetByIdAsync(id);

        if (profile is null)
        {
            return false;
        }

        await EnsureUniqueAsync(request, excludeId: id);

        request.ApplyTo(profile);

        await _repository.UpdateAsync(profile);

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _repository.DeleteAsync(id);
    }

    private async Task EnsureUniqueAsync(ProfileRequest request, Guid? excludeId)
    {
        if (await _repository.EmailExistsAsync(request.Email, excludeId))
        {
            throw new RequestValidationException("email", "This email is already in use.");
        }

        if (await _repository.HandleExistsAsync(request.Handle, excludeId))
        {
            throw new RequestValidationException("handle", "This handle is already in use.");
        }
    }
}
