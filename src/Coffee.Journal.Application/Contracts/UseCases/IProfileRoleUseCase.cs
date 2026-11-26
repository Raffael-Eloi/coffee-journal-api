namespace Coffee.Journal.Application.Contracts.UseCases;

public interface IProfileRoleUseCase
{
    /// <summary>Gives the role to the profile. Does nothing if it already has it. False when the profile does not exist.</summary>
    Task<bool> GrantAsync(Guid profileId, string roleName);

    /// <summary>Takes the role from the profile. Does nothing if it does not have it. False when the profile does not exist.</summary>
    Task<bool> RevokeAsync(Guid profileId, string roleName);
}
