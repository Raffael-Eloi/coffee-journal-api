using System.Linq.Expressions;
using Coffee.Journal.Domain.Contracts.Repositories;
using Coffee.Journal.Domain.Entities;
using Coffee.Journal.Infrastructure.Database.Contexts;
using Microsoft.EntityFrameworkCore;
using CoffeeEntity = Coffee.Journal.Domain.Entities.Coffee;

namespace Coffee.Journal.Infrastructure.Repositories;

internal abstract class ReviewRepository<TReview, TTarget> : BaseRepository<TReview>, IReviewRepository<TReview>
    where TReview : BaseEntity, IReview
    where TTarget : BaseEntity
{
    protected ReviewRepository(CoffeeJournalDbContext dbContext) : base(dbContext)
    {
    }

    /// <summary>Selects the reviews of one target, for example the ones with a given RoasterId.</summary>
    protected abstract Expression<Func<TReview, bool>> ForTarget(Guid targetId);

    public async Task<bool> TargetExistsAsync(Guid targetId)
    {
        return await _dbContext.Set<TTarget>().AnyAsync(x => x.Id == targetId);
    }

    public async Task<bool> ReviewExistsAsync(Guid targetId, Guid profileId)
    {
        return await _dbContext.Set<TReview>()
            .Where(ForTarget(targetId))
            .AnyAsync(x => x.ProfileId == profileId);
    }

    public async Task<IReadOnlyList<ReviewWithAuthor>> GetByTargetAsync(Guid targetId)
    {
        return await _dbContext.Set<TReview>()
            .AsNoTracking()
            .Where(ForTarget(targetId))
            .Join(
                _dbContext.Set<Profile>(),
                review => review.ProfileId,
                profile => profile.Id,
                (review, profile) => new ReviewWithAuthor(
                    review.Id,
                    review.Rating,
                    review.Review,
                    review.Recommend,
                    review.CreatedAt,
                    review.UpdatedAt,
                    profile.Id,
                    profile.Handle,
                    profile.Nickname,
                    profile.PhotoUrl))
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }
}

internal sealed class CoffeeRoasterReviewRepository
    : ReviewRepository<CoffeeRoasterReview, CoffeeRoaster>, ICoffeeRoasterReviewRepository
{
    public CoffeeRoasterReviewRepository(CoffeeJournalDbContext dbContext) : base(dbContext)
    {
    }

    protected override Expression<Func<CoffeeRoasterReview, bool>> ForTarget(Guid targetId) => x => x.RoasterId == targetId;
}

internal sealed class CoffeeReviewRepository
    : ReviewRepository<CoffeeReview, CoffeeEntity>, ICoffeeReviewRepository
{
    public CoffeeReviewRepository(CoffeeJournalDbContext dbContext) : base(dbContext)
    {
    }

    protected override Expression<Func<CoffeeReview, bool>> ForTarget(Guid targetId) => x => x.CoffeeId == targetId;
}

internal sealed class CoffeeShopReviewRepository
    : ReviewRepository<CoffeeShopReview, CoffeeShop>, ICoffeeShopReviewRepository
{
    public CoffeeShopReviewRepository(CoffeeJournalDbContext dbContext) : base(dbContext)
    {
    }

    protected override Expression<Func<CoffeeShopReview, bool>> ForTarget(Guid targetId) => x => x.ShopId == targetId;
}
