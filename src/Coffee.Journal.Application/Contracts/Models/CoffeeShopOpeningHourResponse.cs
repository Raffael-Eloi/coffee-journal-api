namespace Coffee.Journal.Application.Contracts.Models;

public sealed class CoffeeShopOpeningHourResponse
{
    public DayOfWeek DayOfWeek { get; init; }

    public TimeOnly OpensAt { get; init; }

    public TimeOnly ClosesAt { get; init; }
}
