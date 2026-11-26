using Coffee.Journal.Application.Contracts;
using Coffee.Journal.Domain.Contracts.Repositories;
using Coffee.Journal.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace Coffee.Journal.API.Authentication;

/// <summary>The user must have at least one of these roles, as stored in the database.</summary>
internal sealed class RoleRequirement : IAuthorizationRequirement
{
    public RoleRequirement(params string[] roles)
    {
        Roles = roles;
    }

    public IReadOnlyList<string> Roles { get; }
}

/// <summary>Roles are read from the database on every request, so a role change applies at once.</summary>
internal sealed class RoleAuthorizationHandler : AuthorizationHandler<RoleRequirement>
{
    private readonly IProfileRepository _profileRepository;

    public RoleAuthorizationHandler(IProfileRepository profileRepository)
    {
        _profileRepository = profileRepository;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
    {
        string? externalId = CurrentUserClaims.From(context.User).ExternalId;

        if (externalId is null)
        {
            return;
        }

        Profile? profile = await _profileRepository.GetByExternalIdAsync(externalId);

        bool hasRole = profile?.Roles.Any(x => x.Role is not null && requirement.Roles.Contains(x.Role.Name)) == true;

        if (hasRole)
        {
            context.Succeed(requirement);
        }
    }
}
