using Coffee.Journal.Application.Contracts;
using Coffee.Journal.Application.Contracts.UseCases;
using Coffee.Journal.Application.Exceptions;
using Coffee.Journal.Domain.Contracts.Repositories;
using Coffee.Journal.Domain.Entities;

namespace Coffee.Journal.Application.UseCases;

public sealed class ProfileRoleUseCase : IProfileRoleUseCase
{
    private readonly IProfileRepository _profileRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly ICurrentUser _currentUser;

    public ProfileRoleUseCase(
        IProfileRepository profileRepository,
        IRoleRepository roleRepository,
        ICurrentUser currentUser)
    {
        _profileRepository = profileRepository;
        _roleRepository = roleRepository;
        _currentUser = currentUser;
    }

    public async Task<bool> GrantAsync(Guid profileId, string roleName)
    {
        Profile? profile = await _profileRepository.GetByIdAsync(profileId);

        if (profile is null)
        {
            return false;
        }

        Role role = await GetRoleAsync(roleName);

        if (profile.Roles.Any(x => x.RoleId == role.Id))
        {
            return true;
        }

        DateTime now = DateTimeOffset.UtcNow.DateTime;

        profile.Roles.Add(new ProfileRole
        {
            ProfileId = profile.Id,
            RoleId = role.Id,
            CreatedAt = now,
            UpdatedAt = now,
        });

        await _profileRepository.UpdateAsync(profile);

        return true;
    }

    public async Task<bool> RevokeAsync(Guid profileId, string roleName)
    {
        Profile? profile = await _profileRepository.GetByIdAsync(profileId);

        if (profile is null)
        {
            return false;
        }

        Role role = await GetRoleAsync(roleName);

        // An admin who removes their own Admin role could leave nobody able to manage roles.
        if (role.Name == RoleNames.Admin && profile.ExternalId == _currentUser.ExternalId)
        {
            throw new RequestValidationException("role", "You cannot remove your own Admin role.");
        }

        ProfileRole? link = profile.Roles.FirstOrDefault(x => x.RoleId == role.Id);

        if (link is null)
        {
            return true;
        }

        profile.Roles.Remove(link);

        await _profileRepository.UpdateAsync(profile);

        return true;
    }

    private async Task<Role> GetRoleAsync(string roleName)
    {
        return await _roleRepository.GetByNameAsync(roleName)
            ?? throw new RequestValidationException("role", $"Role '{roleName}' does not exist.");
    }
}
