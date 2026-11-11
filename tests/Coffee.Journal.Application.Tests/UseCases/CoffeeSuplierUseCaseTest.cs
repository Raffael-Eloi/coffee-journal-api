using Coffee.Journal.Application.Contracts.Models;
using Coffee.Journal.Application.Contracts.UseCases;
using Coffee.Journal.Application.UseCases;
using Coffee.Journal.Domain.Contracts.Repositories;
using Coffee.Journal.Domain.Entities;
using Moq;

namespace Coffee.Journal.Application.Tests.UseCases;

public class CoffeeSuplierUseCaseTest
{
    private readonly Mock<ICoffeeSupplierRepository> _repositoryMock;
    private readonly ICoffeeSuplierUseCase _useCase;

    public CoffeeSuplierUseCaseTest()
    {
        _repositoryMock = new Mock<ICoffeeSupplierRepository>();
        _useCase = new CoffeeSuplierUseCase(_repositoryMock.Object);
    }

    [Fact]
    public async Task GivenCoffeeSupplier_WhenAdd_ThenTheCoffeeSupplierShouldBeAdded()
    {
        #region Arrange

        CoffeeSupplierRequest request = new()
        {
            Name = "Unique Cafés",
            Website = "https://uniquecafes.com.br",
            Recommend = true,
            HasMembershipOption = true,
            Rate = 5,
            Review = "The best coffee supplier",
            LogoUrl = "https://logo.png"
        };

        #endregion

        #region Act

        await _useCase.AddAsync(request);

        #endregion

        #region Assert

        _repositoryMock
            .Verify(repo => repo.AddAsync(It.Is<CoffeeSupplier>(x => 
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
    public async Task GivenCoffeeSuppliers_WhenGetAll_ThenAllCoffeeSuppliersShouldBeReturned()
    {
        #region Arrange

        List<CoffeeSupplier> suppliers =
        [
            CreateSupplier("Unique Cafés"),
            CreateSupplier("Other Cafés"),
        ];

        _repositoryMock
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(suppliers);

        #endregion

        #region Act

        IReadOnlyList<CoffeeSupplierResponse> response = await _useCase.GetAllAsync();

        #endregion

        #region Assert

        Assert.Equal(2, response.Count);
        Assert.Equal(suppliers.Select(x => x.Id), response.Select(x => x.Id));
        Assert.Equal(suppliers.Select(x => x.Name), response.Select(x => x.Name));

        #endregion
    }

    [Fact]
    public async Task GivenExistingCoffeeSupplier_WhenGetById_ThenTheCoffeeSupplierShouldBeReturned()
    {
        #region Arrange

        CoffeeSupplier supplier = CreateSupplier("Unique Cafés");

        _repositoryMock
            .Setup(repo => repo.GetByIdAsync(supplier.Id))
            .ReturnsAsync(supplier);

        #endregion

        #region Act

        CoffeeSupplierResponse? response = await _useCase.GetByIdAsync(supplier.Id);

        #endregion

        #region Assert

        Assert.NotNull(response);
        Assert.Equal(supplier.Id, response.Id);
        Assert.Equal(supplier.Name, response.Name);
        Assert.Equal(supplier.Website, response.Website);
        Assert.Equal(supplier.Rate, response.Rate);
        Assert.Equal(supplier.Review, response.Review);
        Assert.Equal(supplier.Recommend, response.Recommend);
        Assert.Equal(supplier.HasMembershipOption, response.HasMembershipOption);
        Assert.Equal(supplier.LogoUrl, response.LogoUrl);

        #endregion
    }

    [Fact]
    public async Task GivenNonExistingCoffeeSupplier_WhenGetById_ThenNullShouldBeReturned()
    {
        #region Arrange

        _repositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((CoffeeSupplier?)null);

        #endregion

        #region Act

        CoffeeSupplierResponse? response = await _useCase.GetByIdAsync(Guid.NewGuid());

        #endregion

        #region Assert

        Assert.Null(response);

        #endregion
    }

    [Fact]
    public async Task GivenExistingCoffeeSupplier_WhenUpdate_ThenTheCoffeeSupplierShouldBeUpdated()
    {
        #region Arrange

        CoffeeSupplier supplier = CreateSupplier("Unique Cafés");

        CoffeeSupplierRequest request = new()
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
            .Setup(repo => repo.GetByIdAsync(supplier.Id))
            .ReturnsAsync(supplier);

        #endregion

        #region Act

        bool updated = await _useCase.UpdateAsync(supplier.Id, request);

        #endregion

        #region Assert

        Assert.True(updated);

        _repositoryMock
            .Verify(repo => repo.UpdateAsync(It.Is<CoffeeSupplier>(x =>
                x.Id == supplier.Id &&
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
    public async Task GivenNonExistingCoffeeSupplier_WhenUpdate_ThenFalseShouldBeReturnedAndNothingShouldBeUpdated()
    {
        #region Arrange

        CoffeeSupplierRequest request = new()
        {
            Name = "Unique Cafés",
            Recommend = true,
            HasMembershipOption = true,
            Rate = 5,
            Review = "The best coffee supplier",
        };

        _repositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((CoffeeSupplier?)null);

        #endregion

        #region Act

        bool updated = await _useCase.UpdateAsync(Guid.NewGuid(), request);

        #endregion

        #region Assert

        Assert.False(updated);

        _repositoryMock
            .Verify(repo => repo.UpdateAsync(It.IsAny<CoffeeSupplier>()), Times.Never());

        #endregion
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task GivenCoffeeSupplierId_WhenDelete_ThenTheRepositoryResultShouldBeReturned(bool repositoryResult)
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

    private static CoffeeSupplier CreateSupplier(string name)
    {
        return new CoffeeSupplier
        {
            Name = name,
            Website = "https://uniquecafes.com.br",
            Recommend = true,
            HasMembershipOption = true,
            Rate = 5,
            Review = "The best coffee supplier",
            LogoUrl = "https://logo.png",
        };
    }
}
