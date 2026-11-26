using Coffee.Journal.Application.Contracts.Models;
using Coffee.Journal.Application.Contracts.UseCases;
using Coffee.Journal.Application.Exceptions;
using Coffee.Journal.Application.UseCases;
using Coffee.Journal.Domain.Contracts.Repositories;
using Coffee.Journal.Domain.Contracts.Storage;
using Coffee.Journal.Domain.Entities;
using Coffee.Journal.Domain.Enums;
using Moq;

namespace Coffee.Journal.Application.Tests.UseCases;

public class CoffeeUseCaseTest
{
    private readonly Mock<ICoffeeRepository> _repositoryMock;
    private readonly Mock<ICoffeeRoasterRepository> _roasterRepositoryMock;
    private readonly Mock<IImageStorage> _imageStorageMock;
    private readonly ICoffeeUseCase _useCase;

    public CoffeeUseCaseTest()
    {
        _repositoryMock = new Mock<ICoffeeRepository>();
        _roasterRepositoryMock = new Mock<ICoffeeRoasterRepository>();
        _imageStorageMock = new Mock<IImageStorage>();
        _imageStorageMock
            .Setup(storage => storage.IsStorageUrl(It.IsAny<string>()))
            .Returns(true);
        _useCase = new CoffeeUseCase(
            _repositoryMock.Object,
            _roasterRepositoryMock.Object,
            _imageStorageMock.Object);
    }

    [Fact]
    public async Task GivenCoffee_WhenAdd_ThenTheCoffeeWithPricesAndImagesShouldBeAdded()
    {
        #region Arrange

        CoffeeRoaster roaster = CreateRoaster();
        CoffeeRequest request = CreateRequest(roaster.Id);

        _roasterRepositoryMock
            .Setup(repo => repo.GetByIdAsync(roaster.Id))
            .ReturnsAsync(roaster);

        #endregion

        #region Act

        await _useCase.AddAsync(request);

        #endregion

        #region Assert

        _repositoryMock
            .Verify(repo => repo.AddAsync(It.Is<Domain.Entities.Coffee>(x =>
                x.Name == request.Name &&
                x.Description == request.Description &&
                x.RoasterId == roaster.Id &&
                x.Classification == request.Classification &&
                x.ProductUrl == request.ProductUrl &&
                x.Origin == request.Origin &&
                x.Process == request.Process &&
                x.RoastLevel == request.RoastLevel &&
                x.Variety == request.Variety &&
                x.TastingNotes == request.TastingNotes &&
                x.Prices.Select(price => new { price.Grams, price.Price })
                    .SequenceEqual(request.Prices.Select(price => new { price.Grams, price.Price })) &&
                x.Prices.All(price => price.CoffeeId == x.Id) &&
                x.Images.Select(image => image.Url).SequenceEqual(request.ImageUrls) &&
                x.Images.All(image => image.CoffeeId == x.Id))),
            Times.Once());

        #endregion
    }

    [Fact]
    public async Task GivenNonExistingRoaster_WhenAdd_ThenAValidationExceptionShouldBeThrownAndNothingShouldBeAdded()
    {
        #region Arrange

        CoffeeRequest request = CreateRequest(Guid.NewGuid());

        _roasterRepositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((CoffeeRoaster?)null);

        #endregion

        #region Act

        RequestValidationException exception = await Assert.ThrowsAsync<RequestValidationException>(
            () => _useCase.AddAsync(request));

        #endregion

        #region Assert

        Assert.True(exception.Errors.ContainsKey("roasterId"));

        _repositoryMock
            .Verify(repo => repo.AddAsync(It.IsAny<Domain.Entities.Coffee>()), Times.Never());

        #endregion
    }

    [Fact]
    public async Task GivenCoffees_WhenGetAll_ThenAllCoffeesShouldBeReturned()
    {
        #region Arrange

        List<Domain.Entities.Coffee> coffees =
        [
            CreateCoffee("Unique Coffee", [(250, 40m)], ["https://image1.png"]),
            CreateCoffee("Other Coffee", [], []),
        ];

        _repositoryMock
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(coffees);

        #endregion

        #region Act

        IReadOnlyList<CoffeeResponse> response = await _useCase.GetAllAsync();

        #endregion

        #region Assert

        Assert.Equal(2, response.Count);
        Assert.Equal(coffees.Select(x => x.Id), response.Select(x => x.Id));
        Assert.Equal(coffees.Select(x => x.Name), response.Select(x => x.Name));
        Assert.Single(response[0].Prices);
        Assert.Equal(["https://image1.png"], response[0].ImageUrls);
        Assert.Empty(response[1].Prices);
        Assert.Empty(response[1].ImageUrls);

        #endregion
    }

    [Fact]
    public async Task GivenExistingCoffee_WhenGetById_ThenTheCoffeeShouldBeReturnedWithPricesOrderedByGrams()
    {
        #region Arrange

        Domain.Entities.Coffee coffee = CreateCoffee(
            "Unique Coffee",
            [(1000, 140m), (250, 40m)],
            ["https://image1.png", "https://image2.png"]);

        _repositoryMock
            .Setup(repo => repo.GetByIdAsync(coffee.Id))
            .ReturnsAsync(coffee);

        #endregion

        #region Act

        CoffeeResponse? response = await _useCase.GetByIdAsync(coffee.Id);

        #endregion

        #region Assert

        Assert.NotNull(response);
        Assert.Equal(coffee.Id, response.Id);
        Assert.Equal(coffee.Name, response.Name);
        Assert.Equal(coffee.Description, response.Description);
        Assert.Equal(coffee.RoasterId, response.RoasterId);
        Assert.Equal(coffee.Classification, response.Classification);
        Assert.Equal(coffee.ProductUrl, response.ProductUrl);
        Assert.Equal(coffee.Origin, response.Origin);
        Assert.Equal(coffee.Process, response.Process);
        Assert.Equal(coffee.RoastLevel, response.RoastLevel);
        Assert.Equal(coffee.Variety, response.Variety);
        Assert.Equal(coffee.TastingNotes, response.TastingNotes);
        Assert.Equal([250, 1000], response.Prices.Select(x => x.Grams));
        Assert.Equal([40m, 140m], response.Prices.Select(x => x.Price));
        Assert.Equal(["https://image1.png", "https://image2.png"], response.ImageUrls);

        #endregion
    }

    [Fact]
    public async Task GivenNonExistingCoffee_WhenGetById_ThenNullShouldBeReturned()
    {
        #region Arrange

        _repositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Domain.Entities.Coffee?)null);

        #endregion

        #region Act

        CoffeeResponse? response = await _useCase.GetByIdAsync(Guid.NewGuid());

        #endregion

        #region Assert

        Assert.Null(response);

        #endregion
    }

    [Fact]
    public async Task GivenExistingCoffee_WhenUpdate_ThenTheCoffeeAndItsPricesAndImagesShouldBeUpdated()
    {
        #region Arrange

        CoffeeRoaster roaster = CreateRoaster();
        Domain.Entities.Coffee coffee = CreateCoffee("Unique Coffee", [(250, 40m)], ["https://old-image.png"]);

        CoffeeRequest request = new()
        {
            Name = "Unique Coffee Updated",
            Description = "Updated description",
            RoasterId = roaster.Id,
            Classification = CoffeeClassification.Specialty,
            ProductUrl = "https://store.com/coffee",
            Origin = "Colombia",
            Process = "Washed",
            RoastLevel = "Dark",
            Variety = "Caturra",
            TastingNotes = "Chocolate",
            Prices =
            [
                new CoffeePriceRequest { Grams = 500, Price = 75m, Currency = "BRL" },
                new CoffeePriceRequest { Grams = 1000, Price = 140m, Currency = "BRL" },
            ],
            ImageUrls = ["https://new-image1.png", "https://new-image2.png"],
        };

        _repositoryMock
            .Setup(repo => repo.GetByIdAsync(coffee.Id))
            .ReturnsAsync(coffee);

        _roasterRepositoryMock
            .Setup(repo => repo.GetByIdAsync(roaster.Id))
            .ReturnsAsync(roaster);

        #endregion

        #region Act

        bool updated = await _useCase.UpdateAsync(coffee.Id, request);

        #endregion

        #region Assert

        Assert.True(updated);

        _repositoryMock
            .Verify(repo => repo.UpdateAsync(It.Is<Domain.Entities.Coffee>(x =>
                x.Id == coffee.Id &&
                x.Name == request.Name &&
                x.Description == request.Description &&
                x.RoasterId == roaster.Id &&
                x.Classification == request.Classification &&
                x.ProductUrl == request.ProductUrl &&
                x.Origin == request.Origin &&
                x.Process == request.Process &&
                x.RoastLevel == request.RoastLevel &&
                x.Variety == request.Variety &&
                x.TastingNotes == request.TastingNotes &&
                x.Prices.Select(price => new { price.Grams, price.Price })
                    .SequenceEqual(request.Prices.Select(price => new { price.Grams, price.Price })) &&
                x.Prices.All(price => price.CoffeeId == coffee.Id) &&
                x.Images.Select(image => image.Url).SequenceEqual(request.ImageUrls) &&
                x.Images.All(image => image.CoffeeId == coffee.Id))),
            Times.Once());

        _imageStorageMock
            .Verify(storage => storage.DeleteAsync("https://old-image.png", It.IsAny<CancellationToken>()), Times.Once());
        _imageStorageMock
            .Verify(storage => storage.DeleteAsync("https://new-image1.png", It.IsAny<CancellationToken>()), Times.Never());

        #endregion
    }

    [Fact]
    public async Task GivenNonExistingCoffee_WhenUpdate_ThenFalseShouldBeReturnedAndNothingShouldBeUpdated()
    {
        #region Arrange

        _repositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Domain.Entities.Coffee?)null);

        #endregion

        #region Act

        bool updated = await _useCase.UpdateAsync(Guid.NewGuid(), CreateRequest(Guid.NewGuid()));

        #endregion

        #region Assert

        Assert.False(updated);

        _repositoryMock
            .Verify(repo => repo.UpdateAsync(It.IsAny<Domain.Entities.Coffee>()), Times.Never());

        #endregion
    }

    [Fact]
    public async Task GivenNonExistingRoaster_WhenUpdate_ThenAValidationExceptionShouldBeThrownAndNothingShouldBeUpdated()
    {
        #region Arrange

        Domain.Entities.Coffee coffee = CreateCoffee("Unique Coffee", [], []);

        _repositoryMock
            .Setup(repo => repo.GetByIdAsync(coffee.Id))
            .ReturnsAsync(coffee);

        _roasterRepositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((CoffeeRoaster?)null);

        #endregion

        #region Act

        RequestValidationException exception = await Assert.ThrowsAsync<RequestValidationException>(
            () => _useCase.UpdateAsync(coffee.Id, CreateRequest(Guid.NewGuid())));

        #endregion

        #region Assert

        Assert.True(exception.Errors.ContainsKey("roasterId"));

        _repositoryMock
            .Verify(repo => repo.UpdateAsync(It.IsAny<Domain.Entities.Coffee>()), Times.Never());

        #endregion
    }

    [Fact]
    public async Task GivenExistingCoffee_WhenDelete_ThenTheCoffeeAndItsImagesShouldBeDeleted()
    {
        #region Arrange

        Domain.Entities.Coffee coffee = CreateCoffee("Unique Coffee", [], ["https://image1.png", "https://image2.png"]);

        _repositoryMock
            .Setup(repo => repo.GetByIdAsync(coffee.Id))
            .ReturnsAsync(coffee);

        _repositoryMock
            .Setup(repo => repo.DeleteAsync(coffee.Id))
            .ReturnsAsync(true);

        #endregion

        #region Act

        bool deleted = await _useCase.DeleteAsync(coffee.Id);

        #endregion

        #region Assert

        Assert.True(deleted);

        _repositoryMock
            .Verify(repo => repo.DeleteAsync(coffee.Id), Times.Once());
        _imageStorageMock
            .Verify(storage => storage.DeleteAsync("https://image1.png", It.IsAny<CancellationToken>()), Times.Once());
        _imageStorageMock
            .Verify(storage => storage.DeleteAsync("https://image2.png", It.IsAny<CancellationToken>()), Times.Once());

        #endregion
    }

    [Fact]
    public async Task GivenNonExistingCoffee_WhenDelete_ThenFalseShouldBeReturnedAndNothingShouldBeDeleted()
    {
        #region Arrange

        _repositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Domain.Entities.Coffee?)null);

        #endregion

        #region Act

        bool deleted = await _useCase.DeleteAsync(Guid.NewGuid());

        #endregion

        #region Assert

        Assert.False(deleted);

        _repositoryMock
            .Verify(repo => repo.DeleteAsync(It.IsAny<Guid>()), Times.Never());
        _imageStorageMock
            .Verify(storage => storage.DeleteAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never());

        #endregion
    }

    [Fact]
    public async Task GivenImageUrlFromOutsideTheStorage_WhenAdd_ThenAValidationExceptionShouldBeThrownAndNothingShouldBeAdded()
    {
        #region Arrange

        CoffeeRoaster roaster = CreateRoaster();

        _roasterRepositoryMock
            .Setup(repo => repo.GetByIdAsync(roaster.Id))
            .ReturnsAsync(roaster);

        _imageStorageMock
            .Setup(storage => storage.IsStorageUrl("https://image2.png"))
            .Returns(false);

        #endregion

        #region Act

        RequestValidationException exception = await Assert.ThrowsAsync<RequestValidationException>(
            () => _useCase.AddAsync(CreateRequest(roaster.Id)));

        #endregion

        #region Assert

        Assert.True(exception.Errors.ContainsKey("imageUrls"));

        _repositoryMock
            .Verify(repo => repo.AddAsync(It.IsAny<Domain.Entities.Coffee>()), Times.Never());

        #endregion
    }

    [Fact]
    public void GivenDuplicatedGrams_WhenValidate_ThenAnErrorShouldBeReturned()
    {
        #region Arrange

        CoffeeRequest request = new()
        {
            Prices =
            [
                new CoffeePriceRequest { Grams = 250, Price = 40m, Currency = "BRL" },
                new CoffeePriceRequest { Grams = 250, Price = 45m, Currency = "BRL" },
            ],
        };

        #endregion

        #region Act

        List<System.ComponentModel.DataAnnotations.ValidationResult> errors = request
            .Validate(new System.ComponentModel.DataAnnotations.ValidationContext(request))
            .ToList();

        #endregion

        #region Assert

        Assert.Contains(errors, x => x.MemberNames.Contains(nameof(CoffeeRequest.Prices)));

        #endregion
    }

    [Fact]
    public void GivenInvalidImageUrl_WhenValidate_ThenAnErrorShouldBeReturnedForThatItem()
    {
        #region Arrange

        CoffeeRequest request = new()
        {
            ImageUrls = ["https://valid.png", "not-a-url"],
        };

        #endregion

        #region Act

        List<System.ComponentModel.DataAnnotations.ValidationResult> errors = request
            .Validate(new System.ComponentModel.DataAnnotations.ValidationContext(request))
            .ToList();

        #endregion

        #region Assert

        System.ComponentModel.DataAnnotations.ValidationResult error = Assert.Single(errors);
        Assert.Contains("ImageUrls[1]", error.MemberNames);

        #endregion
    }

    private static CoffeeRoaster CreateRoaster()
    {
        return new CoffeeRoaster
        {
            Name = "Unique Cafés",
            HasMembershipOption = true,
            City = "São Paulo",
            State = "SP",
            Country = "BR",
        };
    }

    private static CoffeeRequest CreateRequest(Guid roasterId)
    {
        return new CoffeeRequest
        {
            Name = "Unique Coffee",
            Description = "A very good coffee",
            RoasterId = roasterId,
            Classification = CoffeeClassification.Specialty,
            ProductUrl = "https://store.com/coffee",
            Origin = "Brazil",
            Process = "Natural",
            RoastLevel = "Medium",
            Variety = "Bourbon",
            TastingNotes = "Caramel, nuts",
            Prices =
            [
                new CoffeePriceRequest { Grams = 250, Price = 40m, Currency = "BRL" },
                new CoffeePriceRequest { Grams = 1000, Price = 140m, Currency = "BRL" },
            ],
            ImageUrls = ["https://image1.png", "https://image2.png"],
        };
    }

    private static Domain.Entities.Coffee CreateCoffee(
        string name,
        List<(int Grams, decimal Price)> prices,
        List<string> imageUrls)
    {
        Domain.Entities.Coffee coffee = new()
        {
            Name = name,
            Description = "A very good coffee",
            RoasterId = Guid.NewGuid(),
            Classification = CoffeeClassification.Specialty,
            ProductUrl = "https://store.com/coffee",
            Origin = "Brazil",
            Process = "Natural",
            RoastLevel = "Medium",
            Variety = "Bourbon",
            TastingNotes = "Caramel, nuts",
        };

        coffee.Prices = prices
            .Select(x => new CoffeePrice { Grams = x.Grams, Price = x.Price, Currency = "BRL", CoffeeId = coffee.Id })
            .ToList();

        coffee.Images = imageUrls
            .Select(url => new CoffeeImage { Url = url, CoffeeId = coffee.Id })
            .ToList();

        return coffee;
    }
}
