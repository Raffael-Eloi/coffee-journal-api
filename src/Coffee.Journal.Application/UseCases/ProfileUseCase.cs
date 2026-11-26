using Coffee.Journal.Application.Contracts;
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
    private readonly ICurrentUser _currentUser;

    public ProfileUseCase(IProfileRepository repository, ICurrentUser currentUser)
    {
        _repository = repository;
        _currentUser = currentUser;
    }

    public async Task<ProfileResponse?> GetCurrentAsync()
    {
        Profile? profile = await _repository.GetByExternalIdAsync(GetExternalId());

        return profile?.ToResponse();
    }

    public async Task<Guid> CreateCurrentAsync(ProfileRequest request)
    {
        string externalId = GetExternalId();

        if (await _repository.ExternalIdExistsAsync(externalId))
        {
            throw new RequestValidationException("profile", "You already have a profile.");
        }

        string email = _currentUser.Email ?? request.Email
            ?? throw new RequestValidationException("email", "Email is required.");

        await EnsureUniqueAsync(email, request.Handle, excludeId: null);

        return await _repository.AddAsync(request.ToEntity(externalId, email));
    }

    public async Task<bool> UpdateCurrentAsync(ProfileRequest request)
    {
        Profile? profile = await _repository.GetByExternalIdAsync(GetExternalId());

        if (profile is null)
        {
            return false;
        }

        if (await _repository.HandleExistsAsync(request.Handle, profile.Id))
        {
            throw new RequestValidationException("handle", "This handle is already in use.");
        }

        request.ApplyTo(profile);

        await _repository.UpdateAsync(profile);

        return true;
    }

    public async Task<bool> DeleteCurrentAsync()
    {
        Profile? profile = await _repository.GetByExternalIdAsync(GetExternalId());

        if (profile is null)
        {
            return false;
        }

        return await _repository.DeleteAsync(profile.Id);
    }

    public async Task<PublicProfileResponse?> GetPublicByIdAsync(Guid id)
    {
        Profile? profile = await _repository.GetByIdAsync(id);

        return profile?.ToPublicResponse();
    }

    private string GetExternalId()
    {
        return _currentUser.ExternalId
            ?? throw new InvalidOperationException("There is no authenticated user for this request.");
    }

    private async Task EnsureUniqueAsync(string email, string handle, Guid? excludeId)
    {
        if (await _repository.EmailExistsAsync(email, excludeId))
        {
            throw new RequestValidationException("email", "This email is already in use.");
        }

        if (await _repository.HandleExistsAsync(handle, excludeId))
        {
            throw new RequestValidationException("handle", "This handle is already in use.");
        }
    }
}
