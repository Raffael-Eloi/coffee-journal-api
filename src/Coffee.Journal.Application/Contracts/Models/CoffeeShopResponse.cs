namespace Coffee.Journal.Application.Contracts.Models;

public sealed class CoffeeShopResponse : BaseResponse
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string? Website { get; init; }

    public Guid? CoffeeSupplierId { get; init; }

    public int Rate { get; init; }

    public string Review { get; init; } = string.Empty;

    public bool Recommend { get; init; }

    public string City { get; init; } = string.Empty;

    public string State { get; init; } = string.Empty;

    public string Country { get; init; } = string.Empty;

    public string Location { get; init; } = string.Empty;

    public IReadOnlyList<string> ImageUrls { get; init; } = [];

    public DateTime CreatedAt { get; init; }

    public DateTime UpdatedAt { get; init; }
}
