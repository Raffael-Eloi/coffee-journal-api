using Coffee.Journal.Domain.Entities;

namespace Coffee.Journal.Domain.Contracts.Repositories;

/// <summary>A review together with the public data of the person who wrote it.</summary>
public sealed record ReviewWithAuthor(
    Guid Id,
    int Rating,
    string Review,
    bool Recommend,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    Guid ProfileId,
    string Handle,
    string? Nickname,
    string? PhotoUrl);

public interface IReviewRepository<TReview> : IBaseRepository<TReview>
    where TReview : BaseEntity, IReview
{
    /// <summary>Whether the roaster, coffee or coffee shop being reviewed exists.</summary>
    Task<bool> TargetExistsAsync(Guid targetId);

    /// <summary>Whether the profile has already reviewed this target.</summary>
    Task<bool> ReviewExistsAsync(Guid targetId, Guid profileId);

    /// <summary>The reviews of a target, newest first.</summary>
    Task<IReadOnlyList<ReviewWithAuthor>> GetByTargetAsync(Guid targetId);
}

public interface ICoffeeRoasterReviewRepository : IReviewRepository<CoffeeRoasterReview>
{
}

public interface ICoffeeReviewRepository : IReviewRepository<CoffeeReview>
{
}

public interface ICoffeeShopReviewRepository : IReviewRepository<CoffeeShopReview>
{
}
