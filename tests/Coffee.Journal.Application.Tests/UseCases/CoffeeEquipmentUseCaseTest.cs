using System.ComponentModel.DataAnnotations;
using Coffee.Journal.Application.Contracts;
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

public class CoffeeEquipmentUseCaseTest
{
    private readonly Mock<ICoffeeEquipmentRepository> _repositoryMock;
    private readonly Mock<ICoffeeEquipmentCategoryRepository> _categoryRepositoryMock;
    private readonly Mock<IProfileRepository> _profileRepositoryMock;
    private readonly Mock<IImageStorage> _imageStorageMock;
    private readonly Mock<ICurrentUser> _currentUserMock;
    private readonly Profile _profile = CreateProfile();
    private readonly ICoffeeEquipmentUseCase _useCase;

    public CoffeeEquipmentUseCaseTest()
    {
        _repositoryMock = new Mock<ICoffeeEquipmentRepository>();
        _categoryRepositoryMock = new Mock<ICoffeeEquipmentCategoryRepository>();
        _profileRepositoryMock = new Mock<IProfileRepository>();
        _profileRepositoryMock
            .Setup(repo => repo.GetByExternalIdAsync("external-1"))
            .ReturnsAsync(_profile);
        _currentUserMock = new Mock<ICurrentUser>();
        _currentUserMock.SetupGet(user => user.ExternalId).Returns("external-1");
        _imageStorageMock = new Mock<IImageStorage>();
        _imageStorageMock
            .Setup(storage => storage.IsStorageUrl(It.IsAny<string>()))
            .Returns(true);
        _useCase = new CoffeeEquipmentUseCase(
            _repositoryMock.Object,
            _categoryRepositoryMock.Object,
            _profileRepositoryMock.Object,
            _imageStorageMock.Object,
            _currentUserMock.Object);
    }

    [Fact]
    public async Task GivenEquipment_WhenAdd_ThenTheEquipmentShouldBeAdded()
    {
        #region Arrange

        CoffeeEquipmentCategory category = CreateCategory();
        CoffeeEquipmentRequest request = CreateRequest(category.Id);

        _categoryRepositoryMock
            .Setup(repo => repo.GetByIdAsync(category.Id))
            .ReturnsAsync(category);

        #endregion

        #region Act

        await _useCase.AddAsync(request);

        #endregion

        #region Assert

        _repositoryMock
            .Verify(repo => repo.AddAsync(It.Is<CoffeeEquipment>(x =>
                x.Name == request.Name &&
                x.Brand == request.Brand &&
                x.Model == request.Model &&
                x.Review == request.Review &&
                x.Rating == request.Rating &&
                x.Recommend == request.Recommend &&
                x.ProfileId == _profile.Id &&
                x.EquipmentCategoryId == category.Id &&
                x.Price == request.Price &&
                x.Currency == request.Currency &&
                x.Link == request.Link &&
                x.FrequencyOfUse == request.FrequencyOfUse &&
                x.ImageUrl == request.ImageUrl &&
                x.PurchaseDate == request.PurchaseDate)),
            Times.Once());

        #endregion
    }

    [Fact]
    public async Task GivenNonExistingCategory_WhenAdd_ThenAValidationExceptionShouldBeThrownAndNothingShouldBeAdded()
    {
        #region Arrange

        _categoryRepositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((CoffeeEquipmentCategory?)null);

        #endregion

        #region Act

        RequestValidationException exception = await Assert.ThrowsAsync<RequestValidationException>(
            () => _useCase.AddAsync(CreateRequest(Guid.NewGuid())));

        #endregion

        #region Assert

        Assert.True(exception.Errors.ContainsKey("equipmentCategoryId"));

        _repositoryMock
            .Verify(repo => repo.AddAsync(It.IsAny<CoffeeEquipment>()), Times.Never());

        #endregion
    }

    [Fact]
    public async Task GivenNonExistingProfile_WhenAdd_ThenAValidationExceptionShouldBeThrownAndNothingShouldBeAdded()
    {
        #region Arrange

        _profileRepositoryMock
            .Setup(repo => repo.GetByExternalIdAsync("external-1"))
            .ReturnsAsync((Profile?)null);

        #endregion

        #region Act

        RequestValidationException exception = await Assert.ThrowsAsync<RequestValidationException>(
            () => _useCase.AddAsync(CreateRequest(Guid.NewGuid())));

        #endregion

        #region Assert

        Assert.True(exception.Errors.ContainsKey("profile"));

        _repositoryMock
            .Verify(repo => repo.AddAsync(It.IsAny<CoffeeEquipment>()), Times.Never());

        #endregion
    }

    [Fact]
    public async Task GivenImageUrlFromOutsideTheStorage_WhenAdd_ThenAValidationExceptionShouldBeThrownAndNothingShouldBeAdded()
    {
        #region Arrange

        CoffeeEquipmentCategory category = CreateCategory();

        _categoryRepositoryMock
            .Setup(repo => repo.GetByIdAsync(category.Id))
            .ReturnsAsync(category);

        _imageStorageMock
            .Setup(storage => storage.IsStorageUrl("https://image.png"))
            .Returns(false);

        #endregion

        #region Act

        RequestValidationException exception = await Assert.ThrowsAsync<RequestValidationException>(
            () => _useCase.AddAsync(CreateRequest(category.Id)));

        #endregion

        #region Assert

        Assert.True(exception.Errors.ContainsKey("imageUrl"));

        _repositoryMock
            .Verify(repo => repo.AddAsync(It.IsAny<CoffeeEquipment>()), Times.Never());

        #endregion
    }

    [Fact]
    public async Task GivenEquipments_WhenGetAll_ThenAllEquipmentsShouldBeReturned()
    {
        #region Arrange

        List<CoffeeEquipment> equipments =
        [
            CreateEquipment("Comandante C40"),
            CreateEquipment("Hario V60"),
        ];

        _repositoryMock
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(equipments);

        #endregion

        #region Act

        IReadOnlyList<CoffeeEquipmentResponse> response = await _useCase.GetAllAsync();

        #endregion

        #region Assert

        Assert.Equal(equipments.Select(x => x.Id), response.Select(x => x.Id));
        Assert.Equal(equipments.Select(x => x.Name), response.Select(x => x.Name));

        #endregion
    }

    [Fact]
    public async Task GivenExistingEquipment_WhenGetById_ThenTheEquipmentShouldBeReturned()
    {
        #region Arrange

        CoffeeEquipment equipment = CreateEquipment("Comandante C40");

        _repositoryMock
            .Setup(repo => repo.GetByIdAsync(equipment.Id))
            .ReturnsAsync(equipment);

        #endregion

        #region Act

        CoffeeEquipmentResponse? response = await _useCase.GetByIdAsync(equipment.Id);

        #endregion

        #region Assert

        Assert.NotNull(response);
        Assert.Equal(equipment.Id, response.Id);
        Assert.Equal(equipment.Name, response.Name);
        Assert.Equal(equipment.Brand, response.Brand);
        Assert.Equal(equipment.Model, response.Model);
        Assert.Equal(equipment.Review, response.Review);
        Assert.Equal(equipment.Rating, response.Rating);
        Assert.Equal(equipment.Recommend, response.Recommend);
        Assert.Equal(equipment.EquipmentCategoryId, response.EquipmentCategoryId);
        Assert.Equal(equipment.Price, response.Price);
        Assert.Equal(equipment.Currency, response.Currency);
        Assert.Equal(equipment.Link, response.Link);
        Assert.Equal(equipment.FrequencyOfUse, response.FrequencyOfUse);
        Assert.Equal(equipment.ImageUrl, response.ImageUrl);
        Assert.Equal(equipment.PurchaseDate, response.PurchaseDate);

        #endregion
    }

    [Fact]
    public async Task GivenNonExistingEquipment_WhenGetById_ThenNullShouldBeReturned()
    {
        #region Arrange

        _repositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((CoffeeEquipment?)null);

        #endregion

        #region Act

        CoffeeEquipmentResponse? response = await _useCase.GetByIdAsync(Guid.NewGuid());

        #endregion

        #region Assert

        Assert.Null(response);

        #endregion
    }

    [Fact]
    public async Task GivenExistingEquipment_WhenUpdate_ThenTheEquipmentShouldBeUpdatedAndTheOldImageDeleted()
    {
        #region Arrange

        CoffeeEquipmentCategory category = CreateCategory();
        CoffeeEquipment equipment = CreateEquipment("Comandante C40");
        equipment.ImageUrl = "https://old-image.png";

        CoffeeEquipmentRequest request = new()
        {
            Name = "Comandante C40 MK4",
            Brand = "Comandante",
            Model = "MK4",
            Review = "Still great",
            Rating = 4,
            Recommend = false,
            EquipmentCategoryId = category.Id,
            FrequencyOfUse = CoffeeEquipmentUseFrequency.Weekly,
            ImageUrl = "https://new-image.png",
        };

        _repositoryMock
            .Setup(repo => repo.GetByIdAsync(equipment.Id))
            .ReturnsAsync(equipment);

        _categoryRepositoryMock
            .Setup(repo => repo.GetByIdAsync(category.Id))
            .ReturnsAsync(category);

        #endregion

        #region Act

        bool updated = await _useCase.UpdateAsync(equipment.Id, request);

        #endregion

        #region Assert

        Assert.True(updated);

        _repositoryMock
            .Verify(repo => repo.UpdateAsync(It.Is<CoffeeEquipment>(x =>
                x.Id == equipment.Id &&
                x.Name == request.Name &&
                x.Model == request.Model &&
                x.Rating == request.Rating &&
                x.Recommend == false &&
                x.EquipmentCategoryId == category.Id &&
                x.Price == null &&
                x.Currency == null &&
                x.FrequencyOfUse == CoffeeEquipmentUseFrequency.Weekly &&
                x.ImageUrl == request.ImageUrl)),
            Times.Once());

        _imageStorageMock
            .Verify(storage => storage.DeleteAsync("https://old-image.png", It.IsAny<CancellationToken>()), Times.Once());
        _imageStorageMock
            .Verify(storage => storage.DeleteAsync("https://new-image.png", It.IsAny<CancellationToken>()), Times.Never());

        #endregion
    }

    [Fact]
    public async Task GivenNonExistingEquipment_WhenUpdate_ThenFalseShouldBeReturnedAndNothingShouldBeUpdated()
    {
        #region Arrange

        _repositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((CoffeeEquipment?)null);

        #endregion

        #region Act

        bool updated = await _useCase.UpdateAsync(Guid.NewGuid(), CreateRequest(Guid.NewGuid()));

        #endregion

        #region Assert

        Assert.False(updated);

        _repositoryMock
            .Verify(repo => repo.UpdateAsync(It.IsAny<CoffeeEquipment>()), Times.Never());

        #endregion
    }

    [Fact]
    public async Task GivenNonExistingCategory_WhenUpdate_ThenAValidationExceptionShouldBeThrownAndNothingShouldBeUpdated()
    {
        #region Arrange

        CoffeeEquipment equipment = CreateEquipment("Comandante C40");

        _repositoryMock
            .Setup(repo => repo.GetByIdAsync(equipment.Id))
            .ReturnsAsync(equipment);

        _categoryRepositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((CoffeeEquipmentCategory?)null);

        #endregion

        #region Act

        RequestValidationException exception = await Assert.ThrowsAsync<RequestValidationException>(
            () => _useCase.UpdateAsync(equipment.Id, CreateRequest(Guid.NewGuid())));

        #endregion

        #region Assert

        Assert.True(exception.Errors.ContainsKey("equipmentCategoryId"));

        _repositoryMock
            .Verify(repo => repo.UpdateAsync(It.IsAny<CoffeeEquipment>()), Times.Never());

        #endregion
    }

    [Fact]
    public async Task GivenExistingEquipment_WhenDelete_ThenTheEquipmentAndItsImageShouldBeDeleted()
    {
        #region Arrange

        CoffeeEquipment equipment = CreateEquipment("Comandante C40");

        _repositoryMock
            .Setup(repo => repo.GetByIdAsync(equipment.Id))
            .ReturnsAsync(equipment);

        _repositoryMock
            .Setup(repo => repo.DeleteAsync(equipment.Id))
            .ReturnsAsync(true);

        #endregion

        #region Act

        bool deleted = await _useCase.DeleteAsync(equipment.Id);

        #endregion

        #region Assert

        Assert.True(deleted);

        _repositoryMock
            .Verify(repo => repo.DeleteAsync(equipment.Id), Times.Once());
        _imageStorageMock
            .Verify(storage => storage.DeleteAsync(equipment.ImageUrl!, It.IsAny<CancellationToken>()), Times.Once());

        #endregion
    }

    [Fact]
    public async Task GivenNonExistingEquipment_WhenDelete_ThenFalseShouldBeReturnedAndNothingShouldBeDeleted()
    {
        #region Arrange

        _repositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((CoffeeEquipment?)null);

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
    public void GivenPriceWithoutCurrency_WhenValidate_ThenAnErrorShouldBeReturned()
    {
        #region Arrange

        CoffeeEquipmentRequest request = new() { Price = 10m };

        #endregion

        #region Act

        List<ValidationResult> errors = request.Validate(new ValidationContext(request)).ToList();

        #endregion

        #region Assert

        Assert.Contains(errors, x => x.MemberNames.Contains(nameof(CoffeeEquipmentRequest.Currency)));

        #endregion
    }

    [Fact]
    public void GivenInvalidCurrency_WhenValidate_ThenAnErrorShouldBeReturned()
    {
        #region Arrange

        CoffeeEquipmentRequest request = new() { Price = 10m, Currency = "brl" };

        #endregion

        #region Act

        List<ValidationResult> errors = request.Validate(new ValidationContext(request)).ToList();

        #endregion

        #region Assert

        Assert.Contains(errors, x => x.MemberNames.Contains(nameof(CoffeeEquipmentRequest.Currency)));

        #endregion
    }

    [Fact]
    public void GivenInvalidLinkAndImageUrl_WhenValidate_ThenAnErrorShouldBeReturnedForEach()
    {
        #region Arrange

        CoffeeEquipmentRequest request = new() { Link = "not-a-url", ImageUrl = "not-a-url" };

        #endregion

        #region Act

        List<ValidationResult> errors = request.Validate(new ValidationContext(request)).ToList();

        #endregion

        #region Assert

        Assert.Contains(errors, x => x.MemberNames.Contains(nameof(CoffeeEquipmentRequest.Link)));
        Assert.Contains(errors, x => x.MemberNames.Contains(nameof(CoffeeEquipmentRequest.ImageUrl)));

        #endregion
    }

    private static Profile CreateProfile()
    {
        return new Profile
        {
            ExternalId = "external-1",
            Name = "Raffael",
            Email = "raffael@example.com",
            Handle = "raffael",
            LanguagePref = "pt-BR",
        };
    }

    private static CoffeeEquipmentCategory CreateCategory()
    {
        return new CoffeeEquipmentCategory
        {
            Name = "Grinder",
            Description = "Burr and blade grinders.",
        };
    }

    [Fact]
    public async Task GivenEquipmentOfAnotherProfile_WhenUpdateOrDelete_ThenAForbiddenExceptionShouldBeThrown()
    {
        #region Arrange

        CoffeeEquipment equipment = CreateEquipment("Comandante C40");
        equipment.ProfileId = Guid.NewGuid();

        _repositoryMock
            .Setup(repo => repo.GetByIdAsync(equipment.Id))
            .ReturnsAsync(equipment);

        #endregion

        #region Act & Assert

        await Assert.ThrowsAsync<ForbiddenException>(() => _useCase.UpdateAsync(equipment.Id, CreateRequest(Guid.NewGuid())));
        await Assert.ThrowsAsync<ForbiddenException>(() => _useCase.DeleteAsync(equipment.Id));

        _repositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<CoffeeEquipment>()), Times.Never());
        _repositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<Guid>()), Times.Never());

        #endregion
    }

    [Fact]
    public async Task GivenAnAdmin_WhenDeleteEquipmentOfAnotherProfile_ThenItShouldBeDeleted()
    {
        #region Arrange

        _profile.Roles.Add(new ProfileRole { Role = new Role { Name = RoleNames.Admin, Description = "Admin" } });

        CoffeeEquipment equipment = CreateEquipment("Comandante C40");
        equipment.ProfileId = Guid.NewGuid();

        _repositoryMock
            .Setup(repo => repo.GetByIdAsync(equipment.Id))
            .ReturnsAsync(equipment);
        _repositoryMock
            .Setup(repo => repo.DeleteAsync(equipment.Id))
            .ReturnsAsync(true);

        #endregion

        #region Act

        bool deleted = await _useCase.DeleteAsync(equipment.Id);

        #endregion

        #region Assert

        Assert.True(deleted);

        #endregion
    }

    private static CoffeeEquipmentRequest CreateRequest(Guid categoryId)
    {
        return new CoffeeEquipmentRequest
        {
            Name = "Comandante C40",
            Brand = "Comandante",
            Model = "MK3",
            Review = "The best manual grinder",
            Rating = 5,
            Recommend = true,
            EquipmentCategoryId = categoryId,
            Price = 250m,
            Currency = "USD",
            Link = "https://example.com/comandante",
            FrequencyOfUse = CoffeeEquipmentUseFrequency.Daily,
            ImageUrl = "https://image.png",
            PurchaseDate = new DateOnly(2025, 1, 15),
        };
    }

    private CoffeeEquipment CreateEquipment(string name)
    {
        return new CoffeeEquipment
        {
            Name = name,
            Brand = "Comandante",
            Model = "MK3",
            Review = "The best manual grinder",
            Rating = 5,
            Recommend = true,
            ProfileId = _profile.Id,
            EquipmentCategoryId = Guid.NewGuid(),
            Price = 250m,
            Currency = "USD",
            Link = "https://example.com/comandante",
            FrequencyOfUse = CoffeeEquipmentUseFrequency.Daily,
            ImageUrl = "https://image.png",
            PurchaseDate = new DateOnly(2025, 1, 15),
        };
    }
}
