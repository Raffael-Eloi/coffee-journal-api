namespace Coffee.Journal.Domain.Entities;

public sealed class CoffeeShop : BaseEntity
{
    public required string Name { get; set; }

    public string? Website { get; set; }

    public Guid? CoffeeSupplierId { get; set; }

    public required int Rate { get; set; }

    public required string Review { get; set; }

    public required bool Recommend { get; set; }

    public required string City { get; set; }

    public required string State { get; set; }

    public required string Country { get; set; }

    public required string Location { get; set; }

    public List<CoffeeShopImage> Images { get; set; } = [];
}
