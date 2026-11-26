using Coffee.Journal.Application.Contracts.Models;

namespace Coffee.Journal.Application.Contracts.UseCases;

public interface IReviewUseCase
{
    /// <summary>The reviews of a target, or null when the target does not exist.</summary>
    Task<ReviewListResponse?> GetAllAsync(Guid targetId);

    /// <summary>Adds the review of the logged-in user. Null when the target does not exist.</summary>
    Task<Guid?> AddAsync(Guid targetId, ReviewRequest request);

    /// <summary>Changes a review. Only its author can do it. False when the review does not exist.</summary>
    Task<bool> UpdateAsync(Guid targetId, Guid reviewId, ReviewRequest request);

    /// <summary>Deletes a review. Its author or an admin can do it. False when the review does not exist.</summary>
    Task<bool> DeleteAsync(Guid targetId, Guid reviewId);
}

public interface ICoffeeRoasterReviewUseCase : IReviewUseCase
{
}

public interface ICoffeeReviewUseCase : IReviewUseCase
{
}

public interface ICoffeeShopReviewUseCase : IReviewUseCase
{
}
