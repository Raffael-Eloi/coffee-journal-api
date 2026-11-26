using System.ComponentModel.DataAnnotations;

namespace Coffee.Journal.Application.Contracts.Models;

public sealed class ProfileRequest : BaseRequest
{
    [MaxLength(200)]
    [Required]
    public string Name { get; init; } = string.Empty;

    [MaxLength(100)]
    public string? Nickname { get; init; }

    [MaxLength(4000)]
    [Url]
    public string? PhotoUrl { get; init; }

    /// <summary>Used only on create, and only when the access token has no email. It is never changed afterwards.</summary>
    [MaxLength(320)]
    [EmailAddress]
    public string? Email { get; init; }

    [RegularExpression("^[a-z0-9_]{3,30}$", ErrorMessage = "Handle must have 3 to 30 lowercase letters, numbers or underscores.")]
    [Required]
    public string Handle { get; init; } = string.Empty;

    [MaxLength(1000)]
    public string? Bio { get; init; }

    [MaxLength(500)]
    [Url]
    public string? InstagramUrl { get; init; }

    [MaxLength(500)]
    [Url]
    public string? FacebookUrl { get; init; }

    [MaxLength(500)]
    [Url]
    public string? LinkedInUrl { get; init; }

    [MaxLength(500)]
    [Url]
    public string? XUrl { get; init; }

    [MaxLength(500)]
    [Url]
    public string? Website { get; init; }

    [RegularExpression("^[a-z]{2}-[A-Z]{2}$", ErrorMessage = "Language must look like pt-BR.")]
    [Required]
    public string LanguagePref { get; init; } = "pt-BR";
}
