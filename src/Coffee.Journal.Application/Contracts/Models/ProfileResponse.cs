namespace Coffee.Journal.Application.Contracts.Models;

public sealed class ProfileResponse : BaseResponse
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string? Nickname { get; init; }

    public string? PhotoUrl { get; init; }

    public string Email { get; init; } = string.Empty;

    public string Handle { get; init; } = string.Empty;

    public string? Bio { get; init; }

    public string? InstagramUrl { get; init; }

    public string? FacebookUrl { get; init; }

    public string? LinkedInUrl { get; init; }

    public string? XUrl { get; init; }

    public string? Website { get; init; }

    public string LanguagePref { get; init; } = string.Empty;

    public IReadOnlyList<string> Roles { get; init; } = [];

    public DateTime CreatedAt { get; init; }

    public DateTime UpdatedAt { get; init; }
}
