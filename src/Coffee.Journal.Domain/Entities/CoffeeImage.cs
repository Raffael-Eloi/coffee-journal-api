namespace Coffee.Journal.Domain.Entities;

public sealed class CoffeeImage : BaseEntity
{
    public required string Url { get; set; }

    public Guid CoffeeId { get; set; }
}
