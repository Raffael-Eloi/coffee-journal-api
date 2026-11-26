using System.ComponentModel.DataAnnotations;
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

public class CoffeeRoasterUseCaseTest
{
    private readonly Mock<ICoffeeRoasterRepository> _repositoryMock;
    private readonly Mock<IImageStorage> _imageStorageMock;
    private readonly ICoffeeRoasterUseCase _useCase;

    public CoffeeRoasterUseCaseTest()
    {
        _repositoryMock = new Mock<ICoffeeRoasterRepository>();
        _imageStorageMock = new Mock<IImageStorage>();
        _imageStorageMock
            .Setup(storage => storage.IsStorageUrl(It.IsAny<string>()))
            .Returns(true);
        _useCase = new CoffeeRoasterUseCase(_repositoryMock.Object, _imageStorageMock.Object);
    }

    [Fact]
    public async Task GivenCoffeeRoaster_WhenAdd_ThenTheCoffeeRoasterWithImagesAndClassificationsShouldBeAdded()
    {
        #region Arrange

        CoffeeRoasterRequest request = CreateRequest();

        #endregion

        #region Act

        await _useCase.AddAsync(request);

        #endregion

        #region Assert

        _repositoryMock
            .Verify(repo => repo.AddAsync(It.Is<CoffeeRoaster>(x =>
                x.Name == request.Name &&
                x.Description == request.Description &&
                x.Website == request.Website &&
                x.InstagramUrl == request.InstagramUrl &&
                x.PhoneNumber == request.PhoneNumber &&
                x.HasMembershipOption == request.HasMembershipOption &&
                x.LogoUrl == request.LogoUrl &&
                x.City == request.City &&
                x.State == request.State &&
                x.Country == request.Country &&
                x.Address == request.Address &&
                x.Images.Select(image => image.Url).SequenceEqual(request.ImageUrls) &&
                x.Images.All(image => image.RoasterId == x.Id) &&
                x.Classifications.Select(c => c.Classification)
                    .SequenceEqual(new[] { CoffeeClassification.Gourmet, CoffeeClassification.Specialty }) &&
                x.Classifications.All(c => c.RoasterId == x.Id))),
            Times.Once());

        #endregion
    }

    [Fact]
    public async Task GivenImageUrlFromOutsideTheStorage_WhenAdd_ThenAValidationExceptionShouldBeThrownAndNothingShouldBeAdded()
    {
        #region Arrange

        _imageStorageMock
            .Setup(storage => storage.IsStorageUrl("https://image2.png"))
            .Returns(false);

        #endregion

        #region Act

        RequestValidationException exception = await Assert.ThrowsAsync<RequestValidationException>(
            () => _useCase.AddAsync(CreateRequest()));

        #endregion

        #region Assert

        Assert.True(exception.Errors.ContainsKey("imageUrls"));

        _repositoryMock
            .Verify(repo => repo.AddAsync(It.IsAny<CoffeeRoaster>()), Times.Never());

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
        Assert.Equal(roaster.Description, response.Description);
        Assert.Equal(roaster.Website, response.Website);
        Assert.Equal(roaster.InstagramUrl, response.InstagramUrl);
        Assert.Equal(roaster.PhoneNumber, response.PhoneNumber);
        Assert.Equal(roaster.HasMembershipOption, response.HasMembershipOption);
        Assert.Equal(roaster.LogoUrl, response.LogoUrl);
        Assert.Equal(roaster.City, response.City);
        Assert.Equal(roaster.State, response.State);
        Assert.Equal(roaster.Country, response.Country);
        Assert.Equal(roaster.Address, response.Address);
        Assert.Equal([CoffeeClassification.Gourmet, CoffeeClassification.Specialty], response.Classifications);
        Assert.Equal(["https://image1.png"], response.ImageUrls);

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
    public async Task GivenExistingCoffeeRoaster_WhenUpdate_ThenTheRoasterImagesAndClassificationsShouldBeUpdated()
    {
        #region Arrange

        CoffeeRoaster roaster = CreateRoaster("Unique Cafés");

        CoffeeRoasterRequest request = new()
        {
            Name = "Unique Cafés Updated",
            HasMembershipOption = false,
            City = "Campinas",
            State = "SP",
            Country = "BR",
            Classifications = [CoffeeClassification.Specialty, CoffeeClassification.Traditional, CoffeeClassification.Traditional],
            ImageUrls = ["https://new-image.png"],
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
                x.Description == null &&
                x.HasMembershipOption == false &&
                x.City == request.City &&
                x.Classifications.Select(c => c.Classification).Order()
                    .SequenceEqual(new[] { CoffeeClassification.Traditional, CoffeeClassification.Specialty }) &&
                x.Images.Select(image => image.Url).SequenceEqual(request.ImageUrls))),
            Times.Once());

        _imageStorageMock
            .Verify(storage => storage.DeleteAsync("https://image1.png", It.IsAny<CancellationToken>()), Times.Once());
        _imageStorageMock
            .Verify(storage => storage.DeleteAsync("https://new-image.png", It.IsAny<CancellationToken>()), Times.Never());

        #endregion
    }

    [Fact]
    public async Task GivenNonExistingCoffeeRoaster_WhenUpdate_ThenFalseShouldBeReturnedAndNothingShouldBeUpdated()
    {
        #region Arrange

        _repositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((CoffeeRoaster?)null);

        #endregion

        #region Act

        bool updated = await _useCase.UpdateAsync(Guid.NewGuid(), CreateRequest());

        #endregion

        #region Assert

        Assert.False(updated);

        _repositoryMock
            .Verify(repo => repo.UpdateAsync(It.IsAny<CoffeeRoaster>()), Times.Never());

        #endregion
    }

    [Fact]
    public async Task GivenExistingCoffeeRoaster_WhenDelete_ThenTheRoasterAndItsImagesShouldBeDeleted()
    {
        #region Arrange

        CoffeeRoaster roaster = CreateRoaster("Unique Cafés");

        _repositoryMock
            .Setup(repo => repo.GetByIdAsync(roaster.Id))
            .ReturnsAsync(roaster);

        _repositoryMock
            .Setup(repo => repo.DeleteAsync(roaster.Id))
            .ReturnsAsync(true);

        #endregion

        #region Act

        bool deleted = await _useCase.DeleteAsync(roaster.Id);

        #endregion

        #region Assert

        Assert.True(deleted);

        _repositoryMock
            .Verify(repo => repo.DeleteAsync(roaster.Id), Times.Once());
        _imageStorageMock
            .Verify(storage => storage.DeleteAsync("https://image1.png", It.IsAny<CancellationToken>()), Times.Once());

        #endregion
    }

    [Fact]
    public async Task GivenNonExistingCoffeeRoaster_WhenDelete_ThenFalseShouldBeReturnedAndNothingShouldBeDeleted()
    {
        #region Arrange

        _repositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((CoffeeRoaster?)null);

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
    public void GivenUndefinedClassificationAndInvalidImageUrl_WhenValidate_ThenAnErrorShouldBeReturnedForEach()
    {
        #region Arrange

        CoffeeRoasterRequest request = new()
        {
            Classifications = [CoffeeClassification.Gourmet, (CoffeeClassification)99],
            ImageUrls = ["https://valid.png", "not-a-url"],
        };

        #endregion

        #region Act

        List<ValidationResult> errors = request.Validate(new ValidationContext(request)).ToList();

        #endregion

        #region Assert

        Assert.Equal(2, errors.Count);
        Assert.Contains(errors, x => x.MemberNames.Contains("Classifications[1]"));
        Assert.Contains(errors, x => x.MemberNames.Contains("ImageUrls[1]"));

        #endregion
    }

    private static CoffeeRoasterRequest CreateRequest()
    {
        return new CoffeeRoasterRequest
        {
            Name = "Unique Cafés",
            Description = "A specialty coffee roaster",
            Website = "https://uniquecafes.com.br",
            InstagramUrl = "https://instagram.com/uniquecafes",
            PhoneNumber = "+5511999999999",
            HasMembershipOption = true,
            LogoUrl = "https://logo.png",
            City = "São Paulo",
            State = "SP",
            Country = "BR",
            Address = "Rua das Flores, 100",
            Classifications = [CoffeeClassification.Gourmet, CoffeeClassification.Specialty, CoffeeClassification.Gourmet],
            ImageUrls = ["https://image1.png", "https://image2.png"],
        };
    }

    private static CoffeeRoaster CreateRoaster(string name)
    {
        CoffeeRoaster roaster = new()
        {
            Name = name,
            Description = "A specialty coffee roaster",
            Website = "https://uniquecafes.com.br",
            InstagramUrl = "https://instagram.com/uniquecafes",
            PhoneNumber = "+5511999999999",
            HasMembershipOption = true,
            LogoUrl = "https://logo.png",
            City = "São Paulo",
            State = "SP",
            Country = "BR",
            Address = "Rua das Flores, 100",
        };

        roaster.Classifications =
        [
            new CoffeeRoasterClassification { RoasterId = roaster.Id, Classification = CoffeeClassification.Specialty },
            new CoffeeRoasterClassification { RoasterId = roaster.Id, Classification = CoffeeClassification.Gourmet },
        ];

        roaster.Images = [new CoffeeRoasterImage { Url = "https://image1.png", RoasterId = roaster.Id }];

        return roaster;
    }
}
