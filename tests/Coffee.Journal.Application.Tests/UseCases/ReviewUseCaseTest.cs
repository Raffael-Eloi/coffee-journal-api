using Coffee.Journal.Application.Contracts;
using Coffee.Journal.Application.Contracts.Models;
using Coffee.Journal.Application.Exceptions;
using Coffee.Journal.Application.UseCases;
using Coffee.Journal.Domain.Contracts.Repositories;
using Coffee.Journal.Domain.Entities;
using Moq;

namespace Coffee.Journal.Application.Tests.UseCases;

public class ReviewUseCaseTest
{
    private readonly Mock<ICoffeeRoasterReviewRepository> _repository = new();
    private readonly Mock<IProfileRepository> _profileRepository = new();
    private readonly Mock<ICurrentUser> _currentUser = new();
    private readonly Profile _profile = CreateProfile();
    private readonly CoffeeRoasterReviewUseCase _useCase;

    public ReviewUseCaseTest()
    {
        _currentUser.SetupGet(x => x.ExternalId).Returns("external-1");
        _profileRepository.Setup(x => x.GetByExternalIdAsync("external-1")).ReturnsAsync(_profile);
        _useCase = new CoffeeRoasterReviewUseCase(_repository.Object, _profileRepository.Object, _currentUser.Object);
    }

    [Fact]
    public async Task GivenReviews_WhenGetAll_ThenTheSummaryShouldBeCalculated()
    {
        #region Arrange

        Guid roasterId = Guid.NewGuid();
        _repository.Setup(x => x.TargetExistsAsync(roasterId)).ReturnsAsync(true);
        _repository.Setup(x => x.GetByTargetAsync(roasterId)).ReturnsAsync(
        [
            CreateReviewWithAuthor(5, true),
            CreateReviewWithAuthor(4, true),
            CreateReviewWithAuthor(2, false),
        ]);

        #endregion

        #region Act

        ReviewListResponse? response = await _useCase.GetAllAsync(roasterId);

        #endregion

        #region Assert

        Assert.NotNull(response);
        Assert.Equal(3, response!.ReviewCount);
        Assert.Equal(2, response.RecommendedCount);
        Assert.Equal(3.7m, response.AverageRating);
        Assert.Equal("reviewer", response.Reviews[0].Author.Handle);

        #endregion
    }

    [Fact]
    public async Task GivenNoReviews_WhenGetAll_ThenTheAverageShouldBeNull()
    {
        #region Arrange

        Guid roasterId = Guid.NewGuid();
        _repository.Setup(x => x.TargetExistsAsync(roasterId)).ReturnsAsync(true);
        _repository.Setup(x => x.GetByTargetAsync(roasterId)).ReturnsAsync([]);

        #endregion

        #region Act

        ReviewListResponse? response = await _useCase.GetAllAsync(roasterId);

        #endregion

        #region Assert

        Assert.NotNull(response);
        Assert.Null(response!.AverageRating);
        Assert.Equal(0, response.ReviewCount);

        #endregion
    }

    [Fact]
    public async Task GivenNonExistingTarget_WhenGetAllOrAdd_ThenNullShouldBeReturned()
    {
        #region Act & Assert

        Assert.Null(await _useCase.GetAllAsync(Guid.NewGuid()));
        Assert.Null(await _useCase.AddAsync(Guid.NewGuid(), CreateRequest()));
        _repository.Verify(x => x.AddAsync(It.IsAny<CoffeeRoasterReview>()), Times.Never);

        #endregion
    }

    [Fact]
    public async Task GivenNewReview_WhenAdd_ThenItShouldBeSavedForTheCurrentProfile()
    {
        #region Arrange

        Guid roasterId = Guid.NewGuid();
        _repository.Setup(x => x.TargetExistsAsync(roasterId)).ReturnsAsync(true);
        CoffeeRoasterReview? saved = null;
        _repository.Setup(x => x.AddAsync(It.IsAny<CoffeeRoasterReview>()))
            .Callback<CoffeeRoasterReview>(r => saved = r)
            .ReturnsAsync((CoffeeRoasterReview r) => r.Id);

        #endregion

        #region Act

        Guid? id = await _useCase.AddAsync(roasterId, CreateRequest());

        #endregion

        #region Assert

        Assert.NotNull(id);
        Assert.NotNull(saved);
        Assert.Equal(roasterId, saved!.RoasterId);
        Assert.Equal(_profile.Id, saved.ProfileId);
        Assert.Equal(5, saved.Rating);
        Assert.Equal("Great", saved.Review);
        Assert.True(saved.Recommend);

        #endregion
    }

    [Fact]
    public async Task GivenAlreadyReviewed_WhenAdd_ThenAValidationExceptionShouldBeThrown()
    {
        #region Arrange

        Guid roasterId = Guid.NewGuid();
        _repository.Setup(x => x.TargetExistsAsync(roasterId)).ReturnsAsync(true);
        _repository.Setup(x => x.ReviewExistsAsync(roasterId, _profile.Id)).ReturnsAsync(true);

        #endregion

        #region Act & Assert

        RequestValidationException exception = await Assert.ThrowsAsync<RequestValidationException>(
            () => _useCase.AddAsync(roasterId, CreateRequest()));
        Assert.True(exception.Errors.ContainsKey("review"));
        _repository.Verify(x => x.AddAsync(It.IsAny<CoffeeRoasterReview>()), Times.Never);

        #endregion
    }

    [Fact]
    public async Task GivenNoProfile_WhenAdd_ThenAValidationExceptionShouldBeThrown()
    {
        #region Arrange

        Guid roasterId = Guid.NewGuid();
        _repository.Setup(x => x.TargetExistsAsync(roasterId)).ReturnsAsync(true);
        _profileRepository.Setup(x => x.GetByExternalIdAsync("external-1")).ReturnsAsync((Profile?)null);

        #endregion

        #region Act & Assert

        RequestValidationException exception = await Assert.ThrowsAsync<RequestValidationException>(
            () => _useCase.AddAsync(roasterId, CreateRequest()));
        Assert.True(exception.Errors.ContainsKey("profile"));

        #endregion
    }

    [Fact]
    public async Task GivenOwnReview_WhenUpdate_ThenItShouldBeChanged()
    {
        #region Arrange

        CoffeeRoasterReview review = CreateReview(_profile.Id);
        _repository.Setup(x => x.GetByIdAsync(review.Id)).ReturnsAsync(review);

        #endregion

        #region Act

        bool updated = await _useCase.UpdateAsync(review.RoasterId, review.Id, CreateRequest());

        #endregion

        #region Assert

        Assert.True(updated);
        Assert.Equal(5, review.Rating);
        Assert.Equal("Great", review.Review);
        _repository.Verify(x => x.UpdateAsync(review), Times.Once);

        #endregion
    }

    [Fact]
    public async Task GivenReviewOfAnotherProfile_WhenUpdate_ThenAForbiddenExceptionShouldBeThrownEvenForAnAdmin()
    {
        #region Arrange

        _profile.Roles.Add(new ProfileRole { Role = new Role { Name = RoleNames.Admin, Description = "Admin" } });
        CoffeeRoasterReview review = CreateReview(Guid.NewGuid());
        _repository.Setup(x => x.GetByIdAsync(review.Id)).ReturnsAsync(review);

        #endregion

        #region Act & Assert

        await Assert.ThrowsAsync<ForbiddenException>(
            () => _useCase.UpdateAsync(review.RoasterId, review.Id, CreateRequest()));
        _repository.Verify(x => x.UpdateAsync(It.IsAny<CoffeeRoasterReview>()), Times.Never);

        #endregion
    }

    [Fact]
    public async Task GivenReviewOfAnotherTarget_WhenUpdateOrDelete_ThenFalseShouldBeReturned()
    {
        #region Arrange

        CoffeeRoasterReview review = CreateReview(_profile.Id);
        _repository.Setup(x => x.GetByIdAsync(review.Id)).ReturnsAsync(review);

        #endregion

        #region Act & Assert

        Assert.False(await _useCase.UpdateAsync(Guid.NewGuid(), review.Id, CreateRequest()));
        Assert.False(await _useCase.DeleteAsync(Guid.NewGuid(), review.Id));

        #endregion
    }

    [Fact]
    public async Task GivenReviewOfAnotherProfile_WhenDelete_ThenAForbiddenExceptionShouldBeThrown()
    {
        #region Arrange

        CoffeeRoasterReview review = CreateReview(Guid.NewGuid());
        _repository.Setup(x => x.GetByIdAsync(review.Id)).ReturnsAsync(review);

        #endregion

        #region Act & Assert

        await Assert.ThrowsAsync<ForbiddenException>(() => _useCase.DeleteAsync(review.RoasterId, review.Id));
        _repository.Verify(x => x.DeleteAsync(It.IsAny<Guid>()), Times.Never);

        #endregion
    }

    [Fact]
    public async Task GivenAnAdmin_WhenDeleteReviewOfAnotherProfile_ThenItShouldBeDeleted()
    {
        #region Arrange

        _profile.Roles.Add(new ProfileRole { Role = new Role { Name = RoleNames.Admin, Description = "Admin" } });
        CoffeeRoasterReview review = CreateReview(Guid.NewGuid());
        _repository.Setup(x => x.GetByIdAsync(review.Id)).ReturnsAsync(review);
        _repository.Setup(x => x.DeleteAsync(review.Id)).ReturnsAsync(true);

        #endregion

        #region Act

        bool deleted = await _useCase.DeleteAsync(review.RoasterId, review.Id);

        #endregion

        #region Assert

        Assert.True(deleted);

        #endregion
    }

    [Fact]
    public async Task GivenNonExistingReview_WhenUpdateOrDelete_ThenFalseShouldBeReturned()
    {
        #region Act & Assert

        Assert.False(await _useCase.UpdateAsync(Guid.NewGuid(), Guid.NewGuid(), CreateRequest()));
        Assert.False(await _useCase.DeleteAsync(Guid.NewGuid(), Guid.NewGuid()));

        #endregion
    }

    [Fact]
    public async Task GivenCoffeeAndShopReviews_WhenAdd_ThenTheTargetShouldBeSetOnTheRightProperty()
    {
        #region Arrange

        Mock<ICoffeeReviewRepository> coffeeRepository = new();
        Mock<ICoffeeShopReviewRepository> shopRepository = new();
        Guid targetId = Guid.NewGuid();
        coffeeRepository.Setup(x => x.TargetExistsAsync(targetId)).ReturnsAsync(true);
        shopRepository.Setup(x => x.TargetExistsAsync(targetId)).ReturnsAsync(true);
        CoffeeReview? coffeeReview = null;
        CoffeeShopReview? shopReview = null;
        coffeeRepository.Setup(x => x.AddAsync(It.IsAny<CoffeeReview>()))
            .Callback<CoffeeReview>(r => coffeeReview = r).ReturnsAsync(Guid.NewGuid());
        shopRepository.Setup(x => x.AddAsync(It.IsAny<CoffeeShopReview>()))
            .Callback<CoffeeShopReview>(r => shopReview = r).ReturnsAsync(Guid.NewGuid());

        #endregion

        #region Act

        await new CoffeeReviewUseCase(coffeeRepository.Object, _profileRepository.Object, _currentUser.Object)
            .AddAsync(targetId, CreateRequest());
        await new CoffeeShopReviewUseCase(shopRepository.Object, _profileRepository.Object, _currentUser.Object)
            .AddAsync(targetId, CreateRequest());

        #endregion

        #region Assert

        Assert.Equal(targetId, coffeeReview!.CoffeeId);
        Assert.Equal(targetId, shopReview!.ShopId);

        #endregion
    }

    private static ReviewRequest CreateRequest()
    {
        return new ReviewRequest { Rating = 5, Review = "Great", Recommend = true };
    }

    private CoffeeRoasterReview CreateReview(Guid profileId)
    {
        return new CoffeeRoasterReview
        {
            RoasterId = Guid.NewGuid(),
            ProfileId = profileId,
            Rating = 3,
            Review = "Old",
            Recommend = false,
        };
    }

    private static ReviewWithAuthor CreateReviewWithAuthor(int rating, bool recommend)
    {
        return new ReviewWithAuthor(
            Guid.NewGuid(), rating, "text", recommend, DateTime.UtcNow, DateTime.UtcNow,
            Guid.NewGuid(), "reviewer", null, null);
    }

    private static Profile CreateProfile()
    {
        return new Profile
        {
            Id = Guid.NewGuid(),
            ExternalId = "external-1",
            Name = "Raffael",
            Email = "raffael@example.com",
            Handle = "raffael",
            LanguagePref = "pt-BR",
        };
    }
}
