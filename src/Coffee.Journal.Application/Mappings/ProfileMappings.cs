using Coffee.Journal.Application.Contracts.Models;
using Coffee.Journal.Domain.Entities;

namespace Coffee.Journal.Application.Mappings;

internal static class ProfileMappings
{
    public static Profile ToEntity(this ProfileRequest request)
    {
        Profile profile = new()
        {
            ExternalId = request.ExternalId,
            Name = request.Name,
            Email = request.Email,
            Handle = request.Handle,
            LanguagePref = request.LanguagePref,
        };

        request.ApplyEditableFieldsTo(profile);

        return profile;
    }

    public static void ApplyTo(this ProfileRequest request, Profile profile)
    {
        profile.Name = request.Name;
        profile.Email = request.Email;
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
