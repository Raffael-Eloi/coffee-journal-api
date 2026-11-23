namespace Coffee.Journal.Domain.Entities;

public sealed class CoffeePrice : BaseEntity
{
    public required int Grams { get; set; }

    public required decimal Price { get; set; }

    public Guid CoffeeId { get; set; }
}
