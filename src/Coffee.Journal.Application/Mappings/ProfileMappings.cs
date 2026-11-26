using Coffee.Journal.Application.Contracts.Models;
using Coffee.Journal.Domain.Entities;

namespace Coffee.Journal.Application.Mappings;

internal static class ProfileMappings
{
    public static Profile ToEntity(this ProfileRequest request, string externalId, string email)
    {
        Profile profile = new()
        {
            ExternalId = externalId,
            Name = request.Name,
            Email = email,
            Handle = request.Handle,
            LanguagePref = request.LanguagePref,
        };

        request.ApplyEditableFieldsTo(profile);

        return profile;
    }

    /// <summary>The email and the external ID come from the identity provider and are never changed here.</summary>
    public static void ApplyTo(this ProfileRequest request, Profile profile)
    {
        profile.Name = request.Name;
        profile.Handle = request.Handle;
        profile.LanguagePref = request.LanguagePref;

        request.ApplyEditableFieldsTo(profile);
    }

    public static ProfileResponse ToResponse(this Profile profile)
    {
        return new ProfileResponse
        {
            Id = profile.Id,
            Name = profile.Name,
            Nickname = profile.Nickname,
            PhotoUrl = profile.PhotoUrl,
            Email = profile.Email,
            Handle = profile.Handle,
            Bio = profile.Bio,
            InstagramUrl = profile.InstagramUrl,
            FacebookUrl = profile.FacebookUrl,
            LinkedInUrl = profile.LinkedInUrl,
            XUrl = profile.XUrl,
            Website = profile.Website,
            LanguagePref = profile.LanguagePref,
            Roles = profile.Roles
                .Where(x => x.Role is not null)
                .Select(x => x.Role!.Name)
                .Order()
                .ToList(),
            CreatedAt = profile.CreatedAt,
            UpdatedAt = profile.UpdatedAt,
        };
    }

    public static PublicProfileResponse ToPublicResponse(this Profile profile)
    {
        return new PublicProfileResponse
        {
            Id = profile.Id,
            Handle = profile.Handle,
            Nickname = profile.Nickname,
            PhotoUrl = profile.PhotoUrl,
            Bio = profile.Bio,
            InstagramUrl = profile.InstagramUrl,
            FacebookUrl = profile.FacebookUrl,
            LinkedInUrl = profile.LinkedInUrl,
            XUrl = profile.XUrl,
            Website = profile.Website,
        };
    }

    private static void ApplyEditableFieldsTo(this ProfileRequest request, Profile profile)
    {
        profile.Nickname = request.Nickname;
        profile.PhotoUrl = request.PhotoUrl;
        profile.Bio = request.Bio;
        profile.InstagramUrl = request.InstagramUrl;
        profile.FacebookUrl = request.FacebookUrl;
        profile.LinkedInUrl = request.LinkedInUrl;
        profile.XUrl = request.XUrl;
        profile.Website = request.Website;
    }
}
