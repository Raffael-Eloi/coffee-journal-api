using Coffee.Journal.Application.Contracts.Models;
using Coffee.Journal.Application.Contracts.UseCases;
using Coffee.Journal.Application.UseCases;
using Coffee.Journal.Domain.Contracts.Repositories;
using Coffee.Journal.Domain.Entities;
using Moq;

namespace Coffee.Journal.Application.Tests.UseCases;

public class CoffeeRoasterUseCaseTest
{
    private readonly Mock<ICoffeeRoasterRepository> _repositoryMock;
    private readonly ICoffeeRoasterUseCase _useCase;

    public CoffeeRoasterUseCaseTest()
    {
        _repositoryMock = new Mock<ICoffeeRoasterRepository>();
        _useCase = new CoffeeRoasterUseCase(_repositoryMock.Object);
    }

    [Fact]
    public async Task GivenCoffeeRoaster_WhenAdd_ThenTheCoffeeRoasterShouldBeAdded()
    {
        #region Arrange

        CoffeeRoasterRequest request = new()
        {
            Name = "Unique Cafés",
            Website = "https://uniquecafes.com.br",
            Recommend = true,
            HasMembershipOption = true,
            Rate = 5,
            Review = "The best coffee roaster",
            LogoUrl = "https://logo.png"
        };

        #endregion

        #region Act

        await _useCase.AddAsync(request);

        #endregion

        #region Assert

        _repositoryMock
            .Verify(repo => repo.AddAsync(It.Is<CoffeeRoaster>(x => 
                x.Name == request.Name &&
                x.Website == request.Website &&
                x.Recommend == request.Recommend &&
                x.HasMembershipOption == request.HasMembershipOption &&
                x.Rate == request.Rate &&
                x.Review == request.Review &&
                x.LogoUrl == request.LogoUrl)),
            Times.Once());

        #endregion
    }

    [Fact]
    public async Task GivenCoffeeRoasters_WhenGetAll_ThenAllCoffeeRoastersShouldBeReturned()
    {
        #region Arrange

        List<CoffeeRoaster> roasters =
        [
            CreateRoaster("Unique Cafés"),
            CreateRoaster("Other Cafés"),
        ];

        _repositoryMock
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(roasters);

        #endregion

        #region Act

        IReadOnlyList<CoffeeRoasterResponse> response = await _useCase.GetAllAsync();

        #endregion

        #region Assert

        Assert.Equal(2, response.Count);
        Assert.Equal(roasters.Select(x => x.Id), response.Select(x => x.Id));
        Assert.Equal(roasters.Select(x => x.Name), response.Select(x => x.Name));

        #endregion
    }

    [Fact]
    public async Task GivenExistingCoffeeRoaster_WhenGetById_ThenTheCoffeeRoasterShouldBeReturned()
    {
        #region Arrange

        CoffeeRoaster roaster = CreateRoaster("Unique Cafés");

        _repositoryMock
            .Setup(repo => repo.GetByIdAsync(roaster.Id))
            .ReturnsAsync(roaster);

        #endregion

        #region Act

        CoffeeRoasterResponse? response = await _useCase.GetByIdAsync(roaster.Id);

        #endregion

        #region Assert

        Assert.NotNull(response);
        Assert.Equal(roaster.Id, response.Id);
        Assert.Equal(roaster.Name, response.Name);
        Assert.Equal(roaster.Website, response.Website);
        Assert.Equal(roaster.Rate, response.Rate);
        Assert.Equal(roaster.Review, response.Review);
        Assert.Equal(roaster.Recommend, response.Recommend);
        Assert.Equal(roaster.HasMembershipOption, response.HasMembershipOption);
        Assert.Equal(roaster.LogoUrl, response.LogoUrl);

        #endregion
    }

    [Fact]
    public async Task GivenNonExistingCoffeeRoaster_WhenGetById_ThenNullShouldBeReturned()
    {
        #region Arrange

        _repositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((CoffeeRoaster?)null);

        #endregion

        #region Act

        CoffeeRoasterResponse? response = await _useCase.GetByIdAsync(Guid.NewGuid());

        #endregion

        #region Assert

        Assert.Null(response);

        #endregion
    }

    [Fact]
    public async Task GivenExistingCoffeeRoaster_WhenUpdate_ThenTheCoffeeRoasterShouldBeUpdated()
    {
        #region Arrange

        CoffeeRoaster roaster = CreateRoaster("Unique Cafés");

        CoffeeRoasterRequest request = new()
        {
            Name = "Unique Cafés Updated",
            Website = "https://updated.com.br",
            Recommend = false,
            HasMembershipOption = false,
            Rate = 3,
            Review = "Updated review",
            LogoUrl = "https://updated-logo.png"
        };

        _repositoryMock
            .Setup(repo => repo.GetByIdAsync(roaster.Id))
            .ReturnsAsync(roaster);

        #endregion

        #region Act

        bool updated = await _useCase.UpdateAsync(roaster.Id, request);

        #endregion

        #region Assert

        Assert.True(updated);

        _repositoryMock
            .Verify(repo => repo.UpdateAsync(It.Is<CoffeeRoaster>(x =>
                x.Id == roaster.Id &&
                x.Name == request.Name &&
                x.Website == request.Website &&
                x.Recommend == request.Recommend &&
                x.HasMembershipOption == request.HasMembershipOption &&
                x.Rate == request.Rate &&
                x.Review == request.Review &&
                x.LogoUrl == request.LogoUrl)),
            Times.Once());

        #endregion
    }

    [Fact]
    public async Task GivenNonExistingCoffeeRoaster_WhenUpdate_ThenFalseShouldBeReturnedAndNothingShouldBeUpdated()
    {
        #region Arrange

        CoffeeRoasterRequest request = new()
        {
            Name = "Unique Cafés",
            Recommend = true,
            HasMembershipOption = true,
            Rate = 5,
            Review = "The best coffee roaster",
        };

        _repositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((CoffeeRoaster?)null);

        #endregion

        #region Act

        bool updated = await _useCase.UpdateAsync(Guid.NewGuid(), request);

        #endregion

        #region Assert

        Assert.False(updated);

        _repositoryMock
            .Verify(repo => repo.UpdateAsync(It.IsAny<CoffeeRoaster>()), Times.Never());

        #endregion
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task GivenCoffeeRoasterId_WhenDelete_ThenTheRepositoryResultShouldBeReturned(bool repositoryResult)
    {
        #region Arrange

        Guid id = Guid.NewGuid();

        _repositoryMock
            .Setup(repo => repo.DeleteAsync(id))
            .ReturnsAsync(repositoryResult);

        #endregion

        #region Act

        bool deleted = await _useCase.DeleteAsync(id);

        #endregion

        #region Assert

        Assert.Equal(repositoryResult, deleted);

        _repositoryMock
            .Verify(repo => repo.DeleteAsync(id), Times.Once());

        #endregion
    }

    private static CoffeeRoaster CreateRoaster(string name)
    {
        return new CoffeeRoaster
        {
            Name = name,
            Website = "https://uniquecafes.com.br",
            Recommend = true,
            HasMembershipOption = true,
            Rate = 5,
            Review = "The best coffee roaster",
            LogoUrl = "https://logo.png",
        };
    }
}
