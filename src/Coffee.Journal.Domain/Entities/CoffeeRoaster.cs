namespace Coffee.Journal.Domain.Entities;

public sealed class CoffeeRoaster : BaseEntity
{
    public required string Name { get; set; }

    public string? Description { get; set; }

    public string? Website { get; set; }

    public string? InstagramUrl { get; set; }

    public string? PhoneNumber { get; set; }

    public required bool HasMembershipOption { get; set; }

    public string? LogoUrl { get; set; }

    public required string City { get; set; }

    public required string State { get; set; }

    public required string Country { get; set; }

    public string? Address { get; set; }

    public List<CoffeeRoasterImage> Images { get; set; } = [];

    public List<CoffeeRoasterClassification> Classifications { get; set; } = [];
}
