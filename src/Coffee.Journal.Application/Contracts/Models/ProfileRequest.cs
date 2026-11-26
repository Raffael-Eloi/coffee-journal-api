using System.ComponentModel.DataAnnotations;

namespace Coffee.Journal.Application.Contracts.Models;

public sealed class ProfileRequest : BaseRequest
{
    // Temporary: once authentication exists, this comes from the access token.
    // It is used only on create and is never changed afterwards.
    [MaxLength(200)]
    [Required]
    public string ExternalId { get; init; } = string.Empty;

    [MaxLength(200)]
    [Required]
    public string Name { get; init; } = string.Empty;

    [MaxLength(100)]
    public string? Nickname { get; init; }

    [MaxLength(4000)]
    [Url]
    public string? PhotoUrl { get; init; }

    [MaxLength(320)]
    [EmailAddress]
    [Required]
    public string Email { get; init; } = string.Empty;

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
