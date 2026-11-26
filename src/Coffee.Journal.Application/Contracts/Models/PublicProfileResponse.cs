namespace Coffee.Journal.Application.Contracts.Models;

/// <summary>What anyone can see about a profile. It never includes the email, real name or roles.</summary>
public sealed class PublicProfileResponse : BaseResponse
{
    public Guid Id { get; init; }

    public string Handle { get; init; } = string.Empty;

    public string? Nickname { get; init; }

    public string? PhotoUrl { get; init; }

    public string? Bio { get; init; }

    public string? InstagramUrl { get; init; }

    public string? FacebookUrl { get; init; }

    public string? LinkedInUrl { get; init; }

    public string? XUrl { get; init; }

    public string? Website { get; init; }
}
