namespace Coffee.Journal.Application.Contracts.Models;

public sealed class CoffeeResponse : BaseResponse
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public Guid RoasterId { get; init; }

    public int Rate { get; init; }

    public string Review { get; init; } = string.Empty;

    public bool Recommend { get; init; }

    public string Origin { get; init; } = string.Empty;

    public string Process { get; init; } = string.Empty;

    public string RoastLevel { get; init; } = string.Empty;

    public string Variety { get; init; } = string.Empty;

    public string TastingNotes { get; init; } = string.Empty;

    public IReadOnlyList<CoffeePriceResponse> Prices { get; init; } = [];

    public IReadOnlyList<string> ImageUrls { get; init; } = [];

    public DateTime CreatedAt { get; init; }

    public DateTime UpdatedAt { get; init; }
}
