using Coffee.Journal.Application.Contracts;
using Coffee.Journal.Application.Contracts.Models;
using Coffee.Journal.Application.Contracts.UseCases;
using Coffee.Journal.Application.Exceptions;
using Coffee.Journal.Application.Mappings;
using Coffee.Journal.Domain.Contracts.Repositories;
using Coffee.Journal.Domain.Entities;

namespace Coffee.Journal.Application.UseCases;

/// <summary>The rules shared by the reviews of roasters, coffees and coffee shops.</summary>
public abstract class ReviewUseCase<TReview> : IReviewUseCase
    where TReview : BaseEntity, IReview
{
    private readonly IReviewRepository<TReview> _repository;
    private readonly IProfileRepository _profileRepository;
    private readonly ICurrentUser _currentUser;

    protected ReviewUseCase(
        IReviewRepository<TReview> repository,
        IProfileRepository profileRepository,
        ICurrentUser currentUser)
    {
        _repository = repository;
        _profileRepository = profileRepository;
        _currentUser = currentUser;
    }

    /// <summary>What is being reviewed, used in messages. For example "roaster".</summary>
    protected abstract string TargetName { get; }

    protected abstract TReview CreateReview(Guid targetId, Guid profileId);

    protected abstract Guid GetTargetId(TReview review);

    public async Task<ReviewListResponse?> GetAllAsync(Guid targetId)
    {
        if (!await _repository.TargetExistsAsync(targetId))
        {
            return null;
        }

        return (await _repository.GetByTargetAsync(targetId)).ToListResponse();
    }

    public async Task<Guid?> AddAsync(Guid targetId, ReviewRequest request)
    {
        if (!await _repository.TargetExistsAsync(targetId))
        {
            return null;
        }

        Profile profile = await GetCurrentProfileAsync();

        if (await _repository.ReviewExistsAsync(targetId, profile.Id))
        {
            throw new RequestValidationException("review", $"You have already reviewed this {TargetName}.");
        }

        TReview review = CreateReview(targetId, profile.Id);
        request.ApplyTo(review);

        return await _repository.AddAsync(review);
    }

    public async Task<bool> UpdateAsync(Guid targetId, Guid reviewId, ReviewRequest request)
    {
        TReview? review = await _repository.GetByIdAsync(reviewId);

        if (review is null || GetTargetId(review) != targetId)
        {
            return false;
        }

        Profile profile = await GetCurrentProfileAsync();

        if (review.ProfileId != profile.Id)
        {
            throw new ForbiddenException("Only the author can change a review.");
        }

        request.ApplyTo(review);

        await _repository.UpdateAsync(review);

        return true;
    }

    public async Task<bool> DeleteAsync(Guid targetId, Guid reviewId)
    {
        TReview? review = await _repository.GetByIdAsync(reviewId);

        if (review is null || GetTargetId(review) != targetId)
        {
            return false;
        }

        Profile profile = await GetCurrentProfileAsync();

        bool isAdmin = profile.Roles.Any(x => x.Role?.Name == RoleNames.Admin);

        if (review.ProfileId != profile.Id && !isAdmin)
        {
            throw new ForbiddenException("Only the author or an admin can delete a review.");
        }

        return await _repository.DeleteAsync(reviewId);
    }

    private async Task<Profile> GetCurrentProfileAsync()
    {
        string externalId = _currentUser.ExternalId
            ?? throw new InvalidOperationException("There is no authenticated user for this request.");

        return await _profileRepository.GetByExternalIdAsync(externalId)
            ?? throw new RequestValidationException("profile", "Create your profile before writing reviews.");
    }
}

public sealed class CoffeeRoasterReviewUseCase : ReviewUseCase<CoffeeRoasterReview>, ICoffeeRoasterReviewUseCase
{
    public CoffeeRoasterReviewUseCase(
        ICoffeeRoasterReviewRepository repository,
        IProfileRepository profileRepository,
        ICurrentUser currentUser) : base(repository, profileRepository, currentUser)
    {
    }

    protected override string TargetName => "roaster";

    protected override CoffeeRoasterReview CreateReview(Guid targetId, Guid profileId) =>
        new() { RoasterId = targetId, ProfileId = profileId, Rating = 0, Review = string.Empty, Recommend = false };

    protected override Guid GetTargetId(CoffeeRoasterReview review) => review.RoasterId;
}

public sealed class CoffeeReviewUseCase : ReviewUseCase<CoffeeReview>, ICoffeeReviewUseCase
{
    public CoffeeReviewUseCase(
        ICoffeeReviewRepository repository,
        IProfileRepository profileRepository,
        ICurrentUser currentUser) : base(repository, profileRepository, currentUser)
    {
    }

    protected override string TargetName => "coffee";

    protected override CoffeeReview CreateReview(Guid targetId, Guid profileId) =>
        new() { CoffeeId = targetId, ProfileId = profileId, Rating = 0, Review = string.Empty, Recommend = false };

    protected override Guid GetTargetId(CoffeeReview review) => review.CoffeeId;
}

public sealed class CoffeeShopReviewUseCase : ReviewUseCase<CoffeeShopReview>, ICoffeeShopReviewUseCase
{
    public CoffeeShopReviewUseCase(
        ICoffeeShopReviewRepository repository,
        IProfileRepository profileRepository,
        ICurrentUser currentUser) : base(repository, profileRepository, currentUser)
    {
    }

    protected override string TargetName => "coffee shop";

    protected override CoffeeShopReview CreateReview(Guid targetId, Guid profileId) =>
        new() { ShopId = targetId, ProfileId = profileId, Rating = 0, Review = string.Empty, Recommend = false };

    protected override Guid GetTargetId(CoffeeShopReview review) => review.ShopId;
}
