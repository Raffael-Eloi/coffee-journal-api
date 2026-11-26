using Coffee.Journal.Domain.Enums;

namespace Coffee.Journal.Domain.Entities;

public sealed class CoffeeShopBrewingMethod
{
    public Guid ShopId { get; set; }

    public CoffeeBrewingMethod BrewingMethod { get; set; }
}
