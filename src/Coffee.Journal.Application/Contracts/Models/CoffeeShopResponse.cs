using Coffee.Journal.Domain.Enums;

namespace Coffee.Journal.Application.Contracts.Models;

public sealed class CoffeeShopResponse : BaseResponse
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }

    public string? Website { get; init; }

    public string? InstagramUrl { get; init; }

    public string? PhoneNumber { get; init; }

    public string City { get; init; } = string.Empty;

    public string State { get; init; } = string.Empty;

    public string Country { get; init; } = string.Empty;

    public string Address { get; init; } = string.Empty;

    public string TimeZone { get; init; } = string.Empty;

    public IReadOnlyList<Guid> RoasterIds { get; init; } = [];

    public IReadOnlyList<CoffeeBrewingMethod> BrewingMethods { get; init; } = [];

    public IReadOnlyList<Amenity> Amenities { get; init; } = [];

    public IReadOnlyList<CoffeeShopOpeningHourResponse> OpeningHours { get; init; } = [];

    public IReadOnlyList<string> ImageUrls { get; init; } = [];

    public DateTime CreatedAt { get; init; }

    public DateTime UpdatedAt { get; init; }
}
