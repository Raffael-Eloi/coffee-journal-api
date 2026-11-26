using Coffee.Journal.Domain.Enums;

namespace Coffee.Journal.Domain.Entities;

public sealed class CoffeeShopAmenity
{
    public Guid ShopId { get; set; }

    public Amenity Amenity { get; set; }
}
