namespace Coffee.Journal.Application.Contracts.Models;

public sealed class CoffeePriceResponse
{
    public int Grams { get; init; }

    public decimal Price { get; init; }
}
