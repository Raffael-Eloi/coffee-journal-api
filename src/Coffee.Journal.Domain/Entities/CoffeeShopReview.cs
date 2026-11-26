namespace Coffee.Journal.Domain.Entities;

public sealed class CoffeeShopReview : BaseEntity, IReview
{
    public required int Rating { get; set; }

    public required string Review { get; set; }

    public required bool Recommend { get; set; }

    public required Guid ShopId { get; set; }

    public required Guid ProfileId { get; set; }
}
