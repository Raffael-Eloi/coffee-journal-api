namespace Coffee.Journal.Domain.Entities;

public sealed class CoffeeReview : BaseEntity
{
    public required int Rating { get; set; }

    public required string Review { get; set; }

    public required bool Recommend { get; set; }

    public required Guid CoffeeId { get; set; }

    // There is no Profile table yet, so this is not a foreign key for now.
    public required Guid ProfileId { get; set; }
}
