namespace Coffee.Journal.Domain.Entities;

public sealed class CoffeeRoasterImage : BaseEntity
{
    public required string Url { get; set; }

    public Guid RoasterId { get; set; }
}
