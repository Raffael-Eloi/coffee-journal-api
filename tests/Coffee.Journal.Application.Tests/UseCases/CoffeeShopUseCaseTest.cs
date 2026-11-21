using Coffee.Journal.Application.Contracts.Models;
using Coffee.Journal.Application.Contracts.UseCases;
using Coffee.Journal.Application.Exceptions;
using Coffee.Journal.Application.UseCases;
using Coffee.Journal.Domain.Contracts.Repositories;
using Coffee.Journal.Domain.Entities;
using Moq;

namespace Coffee.Journal.Application.Tests.UseCases;

public class CoffeeShopUseCaseTest
{
    private readonly Mock<ICoffeeShopRepository> _repositoryMock;
    private readonly Mock<ICoffeeSupplierRepository> _supplierRepositoryMock;
    private readonly ICoffeeShopUseCase _useCase;

    public CoffeeShopUseCaseTest()
    {
        _repositoryMock = new Mock<ICoffeeShopRepository>();
        _supplierRepositoryMock = new Mock<ICoffeeSupplierRepository>();
        _useCase = new CoffeeShopUseCase(_repositoryMock.Object, _supplierRepositoryMock.Object);
    }

    [Fact]
    public async Task GivenCoffeeShopWithSupplier_WhenAdd_ThenTheCoffeeShopShouldBeAdded()
    {
        #region Arrange

        CoffeeSupplier supplier = CreateSupplier();
        CoffeeShopRequest request = CreateRequest(supplier.Id);

        _supplierRepositoryMock
            .Setup(repo => repo.GetByIdAsync(supplier.Id))
            .ReturnsAsync(supplier);

        #endregion

        #region Act

        await _useCase.AddAsync(request);

        #endregion

        #region Assert

        _repositoryMock
            .Verify(repo => repo.AddAsync(It.Is<CoffeeShop>(x =>
                x.Name == request.Name &&
                x.Website == request.Website &&
                x.CoffeeSupplierId == supplier.Id &&
                x.Rate == request.Rate &&
                x.Review == request.Review &&
                x.Recommend == request.Recommend &&
                x.City == request.City &&
                x.State == request.State &&
                x.Country == request.Country &&
                x.Location == request.Location &&
                x.Images.Select(image => image.Url).SequenceEqual(request.ImageUrls) &&
                x.Images.All(image => image.CoffeeShopId == x.Id))),
            Times.Once());

        #endregion
    }

    [Fact]
    public async Task GivenCoffeeShopWithoutSupplier_WhenAdd_ThenTheSupplierShouldNotBeChecked()
    {
        #region Arrange

        CoffeeShopRequest request = CreateRequest(supplierId: null);

        #endregion

        #region Act

        await _useCase.AddAsync(request);

        #endregion

        #region Assert

        _supplierRepositoryMock
            .Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Never());

        _repositoryMock
            .Verify(repo => repo.AddAsync(It.Is<CoffeeShop>(x => x.CoffeeSupplierId == null)), Times.Once());

        #endregion
    }

    [Fact]
    public async Task GivenNonExistingSupplier_WhenAdd_ThenAValidationExceptionShouldBeThrownAndNothingShouldBeAdded()
    {
        #region Arrange

        CoffeeShopRequest request = CreateRequest(Guid.NewGuid());

        _supplierRepositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((CoffeeSupplier?)null);

        #endregion

        #region Act

        RequestValidationException exception = await Assert.ThrowsAsync<RequestValidationException>(
            () => _useCase.AddAsync(request));

        #endregion

        #region Assert

        Assert.True(exception.Errors.ContainsKey("coffeeSupplierId"));

        _repositoryMock
            .Verify(repo => repo.AddAsync(It.IsAny<CoffeeShop>()), Times.Never());

        #endregion
    }

    [Fact]
    public async Task GivenCoffeeShops_WhenGetAll_ThenAllCoffeeShopsShouldBeReturned()
    {
        #region Arrange

        List<CoffeeShop> shops =
        [
            CreateShop("Unique Coffee", ["https://image1.png"]),
            CreateShop("Other Coffee", []),
        ];

        _repositoryMock
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(shops);

        #endregion

        #region Act

        IReadOnlyList<CoffeeShopResponse> response = await _useCase.GetAllAsync();

        #endregion

        #region Assert

        Assert.Equal(2, response.Count);
        Assert.Equal(shops.Select(x => x.Id), response.Select(x => x.Id));
        Assert.Equal(shops.Select(x => x.Name), response.Select(x => x.Name));
        Assert.Equal(["https://image1.png"], response[0].ImageUrls);
        Assert.Empty(response[1].ImageUrls);

        #endregion
    }

    [Fact]
    public async Task GivenExistingCoffeeShop_WhenGetById_ThenTheCoffeeShopShouldBeReturned()
    {
        #region Arrange

        CoffeeShop shop = CreateShop("Unique Coffee", ["https://image1.png", "https://image2.png"]);

        _repositoryMock
            .Setup(repo => repo.GetByIdAsync(shop.Id))
            .ReturnsAsync(shop);

        #endregion

        #region Act

        CoffeeShopResponse? response = await _useCase.GetByIdAsync(shop.Id);

        #endregion

        #region Assert

        Assert.NotNull(response);
        Assert.Equal(shop.Id, response.Id);
        Assert.Equal(shop.Name, response.Name);
        Assert.Equal(shop.Website, response.Website);
        Assert.Equal(shop.CoffeeSupplierId, response.CoffeeSupplierId);
        Assert.Equal(shop.Rate, response.Rate);
        Assert.Equal(shop.Review, response.Review);
        Assert.Equal(shop.Recommend, response.Recommend);
        Assert.Equal(shop.City, response.City);
        Assert.Equal(shop.State, response.State);
        Assert.Equal(shop.Country, response.Country);
        Assert.Equal(shop.Location, response.Location);
        Assert.Equal(["https://image1.png", "https://image2.png"], response.ImageUrls);

        #endregion
    }

    [Fact]
    public async Task GivenNonExistingCoffeeShop_WhenGetById_ThenNullShouldBeReturned()
    {
        #region Arrange

        _repositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((CoffeeShop?)null);

        #endregion

        #region Act

        CoffeeShopResponse? response = await _useCase.GetByIdAsync(Guid.NewGuid());

        #endregion

        #region Assert

        Assert.Null(response);

        #endregion
    }

    [Fact]
    public async Task GivenExistingCoffeeShop_WhenUpdate_ThenTheCoffeeShopAndItsImagesShouldBeUpdated()
    {
        #region Arrange

        CoffeeShop shop = CreateShop("Unique Coffee", ["https://old-image.png"]);

        CoffeeShopRequest request = new()
        {
            Name = "Unique Coffee Updated",
            Website = "https://updated.com.br",
            CoffeeSupplierId = null,
            Rate = 3,
            Review = "Updated review",
            Recommend = false,
            City = "Rio de Janeiro",
            State = "RJ",
            Country = "Brazil",
            Location = "Updated location",
            ImageUrls = ["https://new-image1.png", "https://new-image2.png"],
        };

        _repositoryMock
            .Setup(repo => repo.GetByIdAsync(shop.Id))
            .ReturnsAsync(shop);

        #endregion

        #region Act

        bool updated = await _useCase.UpdateAsync(shop.Id, request);

        #endregion

        #region Assert

        Assert.True(updated);

        _repositoryMock
            .Verify(repo => repo.UpdateAsync(It.Is<CoffeeShop>(x =>
                x.Id == shop.Id &&
                x.Name == request.Name &&
                x.Website == request.Website &&
                x.CoffeeSupplierId == request.CoffeeSupplierId &&
                x.Rate == request.Rate &&
                x.Review == request.Review &&
                x.Recommend == request.Recommend &&
                x.City == request.City &&
                x.State == request.State &&
                x.Country == request.Country &&
                x.Location == request.Location &&
                x.Images.Select(image => image.Url).SequenceEqual(request.ImageUrls) &&
                x.Images.All(image => image.CoffeeShopId == shop.Id))),
            Times.Once());

        #endregion
    }

    [Fact]
    public async Task GivenNonExistingCoffeeShop_WhenUpdate_ThenFalseShouldBeReturnedAndNothingShouldBeUpdated()
    {
        #region Arrange

        _repositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((CoffeeShop?)null);

        #endregion

        #region Act

        bool updated = await _useCase.UpdateAsync(Guid.NewGuid(), CreateRequest(supplierId: null));

        #endregion

        #region Assert

        Assert.False(updated);

        _repositoryMock
            .Verify(repo => repo.UpdateAsync(It.IsAny<CoffeeShop>()), Times.Never());

        #endregion
    }

    [Fact]
    public async Task GivenNonExistingSupplier_WhenUpdate_ThenAValidationExceptionShouldBeThrownAndNothingShouldBeUpdated()
    {
        #region Arrange

        CoffeeShop shop = CreateShop("Unique Coffee", []);

        _repositoryMock
            .Setup(repo => repo.GetByIdAsync(shop.Id))
            .ReturnsAsync(shop);

        _supplierRepositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((CoffeeSupplier?)null);

        #endregion

        #region Act

        RequestValidationException exception = await Assert.ThrowsAsync<RequestValidationException>(
            () => _useCase.UpdateAsync(shop.Id, CreateRequest(Guid.NewGuid())));

        #endregion

        #region Assert

        Assert.True(exception.Errors.ContainsKey("coffeeSupplierId"));

        _repositoryMock
            .Verify(repo => repo.UpdateAsync(It.IsAny<CoffeeShop>()), Times.Never());

        #endregion
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task GivenCoffeeShopId_WhenDelete_ThenTheRepositoryResultShouldBeReturned(bool repositoryResult)
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

    private static CoffeeSupplier CreateSupplier()
    {
        return new CoffeeSupplier
        {
            Name = "Unique Cafés",
            Rate = 5,
            Review = "The best coffee supplier",
            Recommend = true,
            HasMembershipOption = true,
        };
    }

    private static CoffeeShopRequest CreateRequest(Guid? supplierId)
    {
        return new CoffeeShopRequest
        {
            Name = "Unique Coffee",
            Website = "https://uniquecoffee.com.br",
            CoffeeSupplierId = supplierId,
            Rate = 5,
            Review = "The best coffee shop",
            Recommend = true,
            City = "São Paulo",
            State = "SP",
            Country = "Brazil",
            Location = "Rua Augusta, 100",
            ImageUrls = ["https://image1.png", "https://image2.png"],
        };
    }

    private static CoffeeShop CreateShop(string name, List<string> imageUrls)
    {
        CoffeeShop shop = new()
        {
            Name = name,
            Website = "https://uniquecoffee.com.br",
            CoffeeSupplierId = Guid.NewGuid(),
            Rate = 5,
            Review = "The best coffee shop",
            Recommend = true,
            City = "São Paulo",
            State = "SP",
            Country = "Brazil",
            Location = "Rua Augusta, 100",
        };

        shop.Images = imageUrls
            .Select(url => new CoffeeShopImage { Url = url, CoffeeShopId = shop.Id })
            .ToList();

        return shop;
    }
}
