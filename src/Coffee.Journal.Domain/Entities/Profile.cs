namespace Coffee.Journal.Domain.Entities;

public sealed class Profile : BaseEntity
{
    /// <summary>Identifier of the user in the identity provider (the "sub" claim).</summary>
    public required string ExternalId { get; set; }

    public required string Name { get; set; }

    public string? Nickname { get; set; }

    public string? PhotoUrl { get; set; }

    public required string Email { get; set; }

    public required string Handle { get; set; }

    public string? Bio { get; set; }

    public string? InstagramUrl { get; set; }

    public string? FacebookUrl { get; set; }

    public string? LinkedInUrl { get; set; }

    public string? XUrl { get; set; }

    public string? Website { get; set; }

    public required string LanguagePref { get; set; }

    public List<ProfileRole> Roles { get; set; } = [];
}
