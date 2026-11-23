namespace Coffee.Journal.Application.Contracts.Models;

public sealed class CoffeeRoasterResponse : BaseResponse
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string? Website { get; init; }

    public int Rate { get; init; }

    public string Review { get; init; } = string.Empty;

    public bool Recommend { get; init; }

    public bool HasMembershipOption { get; init; }

    public string? LogoUrl { get; init; }

    public DateTime CreatedAt { get; init; }

    public DateTime UpdatedAt { get; init; }
}
