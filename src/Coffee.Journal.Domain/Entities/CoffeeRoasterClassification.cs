using Coffee.Journal.Domain.Enums;

namespace Coffee.Journal.Domain.Entities;

public sealed class CoffeeRoasterClassification
{
    public Guid RoasterId { get; set; }

    public CoffeeClassification Classification { get; set; }
}
