namespace Coffee.Journal.Domain.Entities;

public sealed class CoffeeReview : BaseEntity, IReview
{
    public required int Rating { get; set; }

    public required string Review { get; set; }

    public required bool Recommend { get; set; }

    public required Guid CoffeeId { get; set; }

    public required Guid ProfileId { get; set; }
}
