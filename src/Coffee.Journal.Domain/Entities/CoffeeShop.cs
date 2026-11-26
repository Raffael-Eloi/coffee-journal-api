namespace Coffee.Journal.Domain.Entities;

public sealed class CoffeeShop : BaseEntity
{
    public required string Name { get; set; }

    public string? Description { get; set; }

    public string? Website { get; set; }

    public string? InstagramUrl { get; set; }

    public string? PhoneNumber { get; set; }

    public required string City { get; set; }

    public required string State { get; set; }

    public required string Country { get; set; }

    public required string Address { get; set; }

    /// <summary>IANA time zone name, for example America/Sao_Paulo.</summary>
    public required string TimeZone { get; set; }

    public List<CoffeeShopImage> Images { get; set; } = [];

    public List<CoffeeShopRoaster> Roasters { get; set; } = [];

    public List<CoffeeShopBrewingMethod> BrewingMethods { get; set; } = [];

    public List<CoffeeShopAmenity> Amenities { get; set; } = [];

    public List<CoffeeShopOpeningHour> OpeningHours { get; set; } = [];
}
