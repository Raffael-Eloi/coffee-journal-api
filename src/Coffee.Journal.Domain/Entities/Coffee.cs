using Coffee.Journal.Domain.Enums;

namespace Coffee.Journal.Domain.Entities;

public sealed class Coffee : BaseEntity
{
    public required string Name { get; set; }

    public required string Description { get; set; }

    public required Guid RoasterId { get; set; }

    public required CoffeeClassification Classification { get; set; }

    public required string Origin { get; set; }

    public required string Process { get; set; }

    public required string RoastLevel { get; set; }

    public required string Variety { get; set; }

    public required string TastingNotes { get; set; }

    public string? ProductUrl { get; set; }

    public List<CoffeePrice> Prices { get; set; } = [];

    public List<CoffeeImage> Images { get; set; } = [];
}
