namespace Coffee.Journal.Domain.Entities;

public sealed class CoffeeShopOpeningHour : BaseEntity
{
    public Guid ShopId { get; set; }

    public required DayOfWeek DayOfWeek { get; set; }

    public required TimeOnly OpensAt { get; set; }

    /// <summary>A value earlier than <see cref="OpensAt"/> means the shop closes after midnight.</summary>
    public required TimeOnly ClosesAt { get; set; }
}
