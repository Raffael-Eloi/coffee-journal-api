using System.ComponentModel.DataAnnotations;
using Coffee.Journal.Application.Contracts.Models;
using Coffee.Journal.Application.Contracts.UseCases;
using Coffee.Journal.Application.Exceptions;
using Coffee.Journal.Application.UseCases;
using Coffee.Journal.Domain.Contracts.Repositories;
using Coffee.Journal.Domain.Entities;
using Moq;

namespace Coffee.Journal.Application.Tests.UseCases;

public class ProfileUseCaseTest
{
    private readonly Mock<IProfileRepository> _repositoryMock;
    private readonly IProfileUseCase _useCase;

    public ProfileUseCaseTest()
    {
        _repositoryMock = new Mock<IProfileRepository>();
        _useCase = new ProfileUseCase(_repositoryMock.Object);
    }

    [Fact]
    public async Task GivenProfile_WhenAdd_ThenTheProfileShouldBeAddedWithoutRoles()
    {
        #region Arrange

        ProfileRequest request = CreateRequest();

        #endregion

        #region Act

        await _useCase.AddAsync(request);

        #endregion

        #region Assert

        _repositoryMock
            .Verify(repo => repo.AddAsync(It.Is<Profile>(x =>
                x.ExternalId == request.ExternalId &&
                x.Name == request.Name &&
                x.Nickname == request.Nickname &&
                x.PhotoUrl == request.PhotoUrl &&
                x.Email == request.Email &&
                x.Handle == request.Handle &&
                x.Bio == request.Bio &&
                x.InstagramUrl == request.InstagramUrl &&
                x.LinkedInUrl == request.LinkedInUrl &&
                x.LanguagePref == request.LanguagePref &&
                x.Roles.Count == 0)),
            Times.Once());

        #endregion
    }

    [Theory]
    [InlineData("externalId")]
    [InlineData("email")]
    [InlineData("handle")]
    public async Task GivenAlreadyUsedValue_WhenAdd_ThenAValidationExceptionShouldBeThrownAndNothingShouldBeAdded(string field)
    {
        #region Arrange

        _repositoryMock
            .Setup(repo => repo.ExternalIdExistsAsync(It.IsAny<string>(), It.IsAny<Guid?>()))
            .ReturnsAsync(field == "externalId");
        _repositoryMock
            .Setup(repo => repo.EmailExistsAsync(It.IsAny<string>(), It.IsAny<Guid?>()))
            .ReturnsAsync(field == "email");
        _repositoryMock
            .Setup(repo => repo.HandleExistsAsync(It.IsAny<string>(), It.IsAny<Guid?>()))
            .ReturnsAsync(field == "handle");

        #endregion

        #region Act

        RequestValidationException exception = await Assert.ThrowsAsync<RequestValidationException>(
            () => _useCase.AddAsync(CreateRequest()));

        #endregion

        #region Assert

        Assert.True(exception.Errors.ContainsKey(field));

        _repositoryMock
            .Verify(repo => repo.AddAsync(It.IsAny<Profile>()), Times.Never());

        #endregion
    }

    [Fact]
    public async Task GivenExistingProfile_WhenGetById_ThenTheProfileShouldBeReturnedWithRoleNames()
    {
        #region Arrange

        Profile profile = CreateProfile();
        profile.Roles =
        [
            new ProfileRole { ProfileId = profile.Id, Role = new Role { Name = "Contributor", Description = "x" } },
            new ProfileRole { ProfileId = profile.Id, Role = new Role { Name = "Admin", Description = "x" } },
        ];

        _repositoryMock
            .Setup(repo => repo.GetByIdAsync(profile.Id))
            .ReturnsAsync(profile);

        #endregion

        #region Act

        ProfileResponse? response = await _useCase.GetByIdAsync(profile.Id);

        #endregion

        #region Assert

        Assert.NotNull(response);
        Assert.Equal(profile.Id, response.Id);
        Assert.Equal(profile.Handle, response.Handle);
        Assert.Equal(profile.Email, response.Email);
        Assert.Equal(["Admin", "Contributor"], response.Roles);

        #endregion
    }

    [Fact]
    public async Task GivenNonExistingProfile_WhenGetById_ThenNullShouldBeReturned()
    {
        #region Arrange

        _repositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Profile?)null);

        #endregion

        #region Act

        ProfileResponse? response = await _useCase.GetByIdAsync(Guid.NewGuid());

        #endregion

        #region Assert

        Assert.Null(response);

        #endregion
    }

    [Fact]
    public async Task GivenExistingProfile_WhenUpdate_ThenTheEditableFieldsShouldBeUpdatedAndTheExternalIdKept()
    {
        #region Arrange

        Profile profile = CreateProfile();

        ProfileRequest request = new()
        {
            ExternalId = "another-external-id",
            Name = "Raffael Updated",
            Email = "new@example.com",
            Handle = "new_handle",
            LanguagePref = "en-US",
            Bio = "Coffee lover",
        };

        _repositoryMock
            .Setup(repo => repo.GetByIdAsync(profile.Id))
            .ReturnsAsync(profile);

        #endregion

        #region Act

        bool updated = await _useCase.UpdateAsync(profile.Id, request);

        #endregion

        #region Assert

        Assert.True(updated);

        _repositoryMock
            .Verify(repo => repo.UpdateAsync(It.Is<Profile>(x =>
                x.Id == profile.Id &&
                x.ExternalId == "external-1" &&
                x.Name == request.Name &&
                x.Email == request.Email &&
                x.Handle == request.Handle &&
                x.LanguagePref == request.LanguagePref &&
                x.Bio == request.Bio &&
                x.Nickname == null)),
            Times.Once());

        #endregion
    }

    [Fact]
    public async Task GivenNonExistingProfile_WhenUpdate_ThenFalseShouldBeReturnedAndNothingShouldBeUpdated()
    {
        #region Arrange

        _repositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Profile?)null);

        #endregion

        #region Act

        bool updated = await _useCase.UpdateAsync(Guid.NewGuid(), CreateRequest());

        #endregion

        #region Assert

        Assert.False(updated);

        _repositoryMock
            .Verify(repo => repo.UpdateAsync(It.IsAny<Profile>()), Times.Never());

        #endregion
    }

    [Fact]
    public async Task GivenHandleUsedByAnotherProfile_WhenUpdate_ThenAValidationExceptionShouldBeThrownAndNothingShouldBeUpdated()
    {
        #region Arrange

        Profile profile = CreateProfile();

        _repositoryMock
            .Setup(repo => repo.GetByIdAsync(profile.Id))
            .ReturnsAsync(profile);

        _repositoryMock
            .Setup(repo => repo.HandleExistsAsync("taken", profile.Id))
            .ReturnsAsync(true);

        #endregion

        #region Act

        RequestValidationException exception = await Assert.ThrowsAsync<RequestValidationException>(
            () => _useCase.UpdateAsync(profile.Id, CreateRequest("taken")));

        #endregion

        #region Assert

        Assert.True(exception.Errors.ContainsKey("handle"));

        _repositoryMock
            .Verify(repo => repo.UpdateAsync(It.IsAny<Profile>()), Times.Never());

        #endregion
    }

    [Theory]
    [InlineData("ab")]
    [InlineData("Has_Upper")]
    [InlineData("has space")]
    public void GivenInvalidHandle_WhenValidate_ThenAnErrorShouldBeReturned(string handle)
    {
        #region Arrange

        ProfileRequest request = new() { Handle = handle };

        #endregion

        #region Act

        List<ValidationResult> errors = [];
        Validator.TryValidateObject(request, new ValidationContext(request), errors, validateAllProperties: true);

        #endregion

        #region Assert

        Assert.Contains(errors, x => x.MemberNames.Contains(nameof(ProfileRequest.Handle)));

        #endregion
    }

    private static ProfileRequest CreateRequest(string handle = "raffael")
    {
        return new ProfileRequest
        {
            ExternalId = "external-1",
            Name = "Raffael",
            Nickname = "Raff",
            PhotoUrl = "https://photo.png",
            Email = "raffael@example.com",
            Handle = handle,
            Bio = "Coffee lover",
            InstagramUrl = "https://instagram.com/raffael",
            LinkedInUrl = "https://linkedin.com/in/raffael",
            LanguagePref = "pt-BR",
        };
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
}
