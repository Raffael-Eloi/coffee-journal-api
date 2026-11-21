namespace Coffee.Journal.Domain.Entities;

public sealed class CoffeeShopImage : BaseEntity
{
    public required string Url { get; set; }

    public Guid CoffeeShopId { get; set; }
}
