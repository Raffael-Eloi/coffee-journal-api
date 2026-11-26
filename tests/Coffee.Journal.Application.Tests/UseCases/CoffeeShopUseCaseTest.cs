using System.ComponentModel.DataAnnotations;
using Coffee.Journal.Application.Contracts.Models;
using Coffee.Journal.Application.Contracts.UseCases;
using Coffee.Journal.Application.Exceptions;
using Coffee.Journal.Application.UseCases;
using Coffee.Journal.Domain.Contracts.Repositories;
using Coffee.Journal.Domain.Entities;
using Coffee.Journal.Domain.Enums;
using Moq;

namespace Coffee.Journal.Application.Tests.UseCases;

public class CoffeeShopUseCaseTest
{
    private readonly Mock<ICoffeeShopRepository> _repositoryMock;
    private readonly Mock<ICoffeeRoasterRepository> _roasterRepositoryMock;
    private readonly ICoffeeShopUseCase _useCase;

    public CoffeeShopUseCaseTest()
    {
        _repositoryMock = new Mock<ICoffeeShopRepository>();
        _roasterRepositoryMock = new Mock<ICoffeeRoasterRepository>();
        _useCase = new CoffeeShopUseCase(_repositoryMock.Object, _roasterRepositoryMock.Object);
    }

    [Fact]
    public async Task GivenCoffeeShop_WhenAdd_ThenTheShopWithItsRelatedDataShouldBeAdded()
    {
        #region Arrange

        CoffeeRoaster roaster = CreateRoaster();
        CoffeeShopRequest request = CreateRequest([roaster.Id]);

        _roasterRepositoryMock
            .Setup(repo => repo.GetByIdAsync(roaster.Id))
            .ReturnsAsync(roaster);

        #endregion

        #region Act

        await _useCase.AddAsync(request);

        #endregion

        #region Assert

        _repositoryMock
            .Verify(repo => repo.AddAsync(It.Is<CoffeeShop>(x =>
                x.Name == request.Name &&
                x.Description == request.Description &&
                x.Website == request.Website &&
                x.InstagramUrl == request.InstagramUrl &&
                x.PhoneNumber == request.PhoneNumber &&
                x.City == request.City &&
                x.State == request.State &&
                x.Country == request.Country &&
                x.Address == request.Address &&
                x.TimeZone == request.TimeZone &&
                x.Roasters.Select(r => r.RoasterId).SequenceEqual(new[] { roaster.Id }) &&
                x.Roasters.All(r => r.ShopId == x.Id) &&
                x.BrewingMethods.Select(b => b.BrewingMethod)
                    .SequenceEqual(new[] { CoffeeBrewingMethod.V60, CoffeeBrewingMethod.Espresso }) &&
                x.Amenities.Select(a => a.Amenity)
                    .SequenceEqual(new[] { Amenity.WiFi, Amenity.PetFriendly }) &&
                x.OpeningHours.Count == 2 &&
                x.OpeningHours.All(h => h.ShopId == x.Id) &&
                x.Images.Select(image => image.Url).SequenceEqual(request.ImageUrls) &&
                x.Images.All(image => image.ShopId == x.Id))),
            Times.Once());

        #endregion
    }

    [Fact]
    public async Task GivenCoffeeShopWithoutRoasters_WhenAdd_ThenTheRoasterShouldNotBeChecked()
    {
        #region Arrange

        CoffeeShopRequest request = CreateRequest([]);

        #endregion

        #region Act

        await _useCase.AddAsync(request);

        #endregion

        #region Assert

        _roasterRepositoryMock
            .Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Never());

        _repositoryMock
            .Verify(repo => repo.AddAsync(It.Is<CoffeeShop>(x => x.Roasters.Count == 0)), Times.Once());

        #endregion
    }

    [Fact]
    public async Task GivenNonExistingRoaster_WhenAdd_ThenAValidationExceptionShouldBeThrownAndNothingShouldBeAdded()
    {
        #region Arrange

        _roasterRepositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((CoffeeRoaster?)null);

        #endregion

        #region Act

        RequestValidationException exception = await Assert.ThrowsAsync<RequestValidationException>(
            () => _useCase.AddAsync(CreateRequest([Guid.NewGuid()])));

        #endregion

        #region Assert

        Assert.True(exception.Errors.ContainsKey("roasterIds"));

        _repositoryMock
            .Verify(repo => repo.AddAsync(It.IsAny<CoffeeShop>()), Times.Never());

        #endregion
    }

    [Fact]
    public async Task GivenCoffeeShops_WhenGetAll_ThenAllCoffeeShopsShouldBeReturned()
    {
        #region Arrange

        List<CoffeeShop> shops = [CreateShop("Unique Coffee"), CreateShop("Other Coffee")];

        _repositoryMock
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(shops);

        #endregion

        #region Act

        IReadOnlyList<CoffeeShopResponse> response = await _useCase.GetAllAsync();

        #endregion

        #region Assert

        Assert.Equal(shops.Select(x => x.Id), response.Select(x => x.Id));
        Assert.Equal(shops.Select(x => x.Name), response.Select(x => x.Name));

        #endregion
    }

    [Fact]
    public async Task GivenExistingCoffeeShop_WhenGetById_ThenTheShopShouldBeReturnedWithOrderedOpeningHours()
    {
        #region Arrange

        CoffeeShop shop = CreateShop("Unique Coffee");

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
        Assert.Equal(shop.Address, response.Address);
        Assert.Equal(shop.TimeZone, response.TimeZone);
        Assert.Equal(shop.Roasters.Select(x => x.RoasterId), response.RoasterIds);
        Assert.Equal([CoffeeBrewingMethod.V60, CoffeeBrewingMethod.Espresso], response.BrewingMethods);
        Assert.Equal([Amenity.WiFi, Amenity.PetFriendly], response.Amenities);
        Assert.Equal([DayOfWeek.Monday, DayOfWeek.Tuesday], response.OpeningHours.Select(x => x.DayOfWeek));
        Assert.Equal(["https://image1.png"], response.ImageUrls);

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
    public async Task GivenExistingCoffeeShop_WhenUpdate_ThenTheShopAndItsRelatedDataShouldBeUpdated()
    {
        #region Arrange

        CoffeeShop shop = CreateShop("Unique Coffee");
        CoffeeRoaster roaster = CreateRoaster();

        CoffeeShopRequest request = new()
        {
            Name = "Unique Coffee Updated",
            City = "Campinas",
            State = "SP",
            Country = "BR",
            Address = "Rua Nova, 10",
            TimeZone = "America/Sao_Paulo",
            RoasterIds = [roaster.Id],
            BrewingMethods = [CoffeeBrewingMethod.Espresso, CoffeeBrewingMethod.Chemex],
            Amenities = [Amenity.WiFi],
            OpeningHours =
            [
                new CoffeeShopOpeningHourRequest
                {
                    DayOfWeek = DayOfWeek.Friday,
                    OpensAt = new TimeOnly(18, 0),
                    ClosesAt = new TimeOnly(2, 0),
                },
            ],
            ImageUrls = ["https://new-image.png"],
        };

        _repositoryMock
            .Setup(repo => repo.GetByIdAsync(shop.Id))
            .ReturnsAsync(shop);

        _roasterRepositoryMock
            .Setup(repo => repo.GetByIdAsync(roaster.Id))
            .ReturnsAsync(roaster);

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
                x.Description == null &&
                x.Address == request.Address &&
                x.Roasters.Select(r => r.RoasterId).SequenceEqual(new[] { roaster.Id }) &&
                x.BrewingMethods.Select(b => b.BrewingMethod).Order()
                    .SequenceEqual(new[] { CoffeeBrewingMethod.Espresso, CoffeeBrewingMethod.Chemex }) &&
                x.Amenities.Select(a => a.Amenity).SequenceEqual(new[] { Amenity.WiFi }) &&
                x.OpeningHours.Count == 1 &&
                x.OpeningHours[0].DayOfWeek == DayOfWeek.Friday &&
                x.Images.Select(image => image.Url).SequenceEqual(request.ImageUrls) &&
                x.Images.All(image => image.ShopId == shop.Id))),
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

        bool updated = await _useCase.UpdateAsync(Guid.NewGuid(), CreateRequest([]));

        #endregion

        #region Assert

        Assert.False(updated);

        _repositoryMock
            .Verify(repo => repo.UpdateAsync(It.IsAny<CoffeeShop>()), Times.Never());

        #endregion
    }

    [Fact]
    public async Task GivenNonExistingRoaster_WhenUpdate_ThenAValidationExceptionShouldBeThrownAndNothingShouldBeUpdated()
    {
        #region Arrange

        CoffeeShop shop = CreateShop("Unique Coffee");

        _repositoryMock
            .Setup(repo => repo.GetByIdAsync(shop.Id))
            .ReturnsAsync(shop);

        _roasterRepositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((CoffeeRoaster?)null);

        #endregion

        #region Act

        RequestValidationException exception = await Assert.ThrowsAsync<RequestValidationException>(
            () => _useCase.UpdateAsync(shop.Id, CreateRequest([Guid.NewGuid()])));

        #endregion

        #region Assert

        Assert.True(exception.Errors.ContainsKey("roasterIds"));

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

        #endregion
    }

    [Fact]
    public void GivenInvalidData_WhenValidate_ThenAnErrorShouldBeReturnedForEachProblem()
    {
        #region Arrange

        CoffeeShopRequest request = new()
        {
            TimeZone = "not a zone!",
            BrewingMethods = [(CoffeeBrewingMethod)99],
            Amenities = [(Amenity)99],
            OpeningHours =
            [
                new CoffeeShopOpeningHourRequest
                {
                    DayOfWeek = DayOfWeek.Monday,
                    OpensAt = new TimeOnly(8, 0),
                    ClosesAt = new TimeOnly(8, 0),
                },
                new CoffeeShopOpeningHourRequest
                {
                    DayOfWeek = DayOfWeek.Tuesday,
                    OpensAt = new TimeOnly(8, 0),
                    ClosesAt = new TimeOnly(12, 0),
                },
                new CoffeeShopOpeningHourRequest
                {
                    DayOfWeek = DayOfWeek.Tuesday,
                    OpensAt = new TimeOnly(8, 0),
                    ClosesAt = new TimeOnly(18, 0),
                },
            ],
            ImageUrls = ["not-a-url"],
        };

        #endregion

        #region Act

        List<ValidationResult> errors = request.Validate(new ValidationContext(request)).ToList();

        #endregion

        #region Assert

        Assert.Contains(errors, x => x.MemberNames.Contains(nameof(CoffeeShopRequest.TimeZone)));
        Assert.Contains(errors, x => x.MemberNames.Contains("BrewingMethods[0]"));
        Assert.Contains(errors, x => x.MemberNames.Contains("Amenities[0]"));
        Assert.Contains(errors, x => x.MemberNames.Contains("OpeningHours[0]"));
        Assert.Contains(errors, x => x.MemberNames.Contains(nameof(CoffeeShopRequest.OpeningHours)));
        Assert.Contains(errors, x => x.MemberNames.Contains("ImageUrls[0]"));

        #endregion
    }

    [Fact]
    public void GivenValidRequest_WhenValidate_ThenNoErrorShouldBeReturned()
    {
        #region Arrange

        CoffeeShopRequest request = CreateRequest([]);

        #endregion

        #region Act

        List<ValidationResult> errors = request.Validate(new ValidationContext(request)).ToList();

        #endregion

        #region Assert

        Assert.Empty(errors);

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

    private static CoffeeShopRequest CreateRequest(List<Guid> roasterIds)
    {
        return new CoffeeShopRequest
        {
            Name = "Unique Coffee",
            Description = "A cozy coffee shop",
            Website = "https://uniquecoffee.com.br",
            InstagramUrl = "https://instagram.com/uniquecoffee",
            PhoneNumber = "+5511999999999",
            City = "São Paulo",
            State = "SP",
            Country = "BR",
            Address = "Rua das Flores, 100",
            TimeZone = "America/Sao_Paulo",
            RoasterIds = roasterIds,
            BrewingMethods = [CoffeeBrewingMethod.V60, CoffeeBrewingMethod.Espresso, CoffeeBrewingMethod.V60],
            Amenities = [Amenity.WiFi, Amenity.PetFriendly],
            OpeningHours =
            [
                new CoffeeShopOpeningHourRequest
                {
                    DayOfWeek = DayOfWeek.Monday,
                    OpensAt = new TimeOnly(8, 0),
                    ClosesAt = new TimeOnly(12, 0),
                },
                new CoffeeShopOpeningHourRequest
                {
                    DayOfWeek = DayOfWeek.Monday,
                    OpensAt = new TimeOnly(14, 0),
                    ClosesAt = new TimeOnly(18, 0),
                },
            ],
            ImageUrls = ["https://image1.png", "https://image2.png"],
        };
    }

    private static CoffeeShop CreateShop(string name)
    {
        CoffeeShop shop = new()
        {
            Name = name,
            Description = "A cozy coffee shop",
            Website = "https://uniquecoffee.com.br",
            City = "São Paulo",
            State = "SP",
            Country = "BR",
            Address = "Rua das Flores, 100",
            TimeZone = "America/Sao_Paulo",
        };

        shop.Roasters = [new CoffeeShopRoaster { RoasterId = Guid.NewGuid(), ShopId = shop.Id }];

        shop.BrewingMethods =
        [
            new CoffeeShopBrewingMethod { ShopId = shop.Id, BrewingMethod = CoffeeBrewingMethod.Espresso },
            new CoffeeShopBrewingMethod { ShopId = shop.Id, BrewingMethod = CoffeeBrewingMethod.V60 },
        ];

        shop.Amenities =
        [
            new CoffeeShopAmenity { ShopId = shop.Id, Amenity = Amenity.PetFriendly },
            new CoffeeShopAmenity { ShopId = shop.Id, Amenity = Amenity.WiFi },
        ];

        shop.OpeningHours =
        [
            new CoffeeShopOpeningHour
            {
                ShopId = shop.Id,
                DayOfWeek = DayOfWeek.Tuesday,
                OpensAt = new TimeOnly(8, 0),
                ClosesAt = new TimeOnly(18, 0),
            },
            new CoffeeShopOpeningHour
            {
                ShopId = shop.Id,
                DayOfWeek = DayOfWeek.Monday,
                OpensAt = new TimeOnly(8, 0),
                ClosesAt = new TimeOnly(18, 0),
            },
        ];

        shop.Images = [new CoffeeShopImage { Url = "https://image1.png", ShopId = shop.Id }];

        return shop;
    }
}
