using Coffee.Journal.Application.Contracts.Models;

namespace Coffee.Journal.Application.Contracts.UseCases;

public interface IProfileUseCase
{
    /// <summary>The profile of the logged-in user, or null when the user has not created one yet.</summary>
    Task<ProfileResponse?> GetCurrentAsync();

    Task<Guid> CreateCurrentAsync(ProfileRequest request);

    Task<bool> UpdateCurrentAsync(ProfileRequest request);

    Task<bool> DeleteCurrentAsync();

    Task<PublicProfileResponse?> GetPublicByIdAsync(Guid id);
}
