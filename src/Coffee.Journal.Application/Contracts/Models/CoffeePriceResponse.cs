namespace Coffee.Journal.Application.Contracts.Models;

public sealed class CoffeePriceResponse
{
    public int Grams { get; init; }

    public decimal Price { get; init; }

    public string Currency { get; init; } = string.Empty;
}
