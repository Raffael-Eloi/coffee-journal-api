using Coffee.Journal.Application.Contracts;
using Coffee.Journal.Application.Contracts.Models;
using Coffee.Journal.Application.Exceptions;
using Coffee.Journal.Application.UseCases;
using Coffee.Journal.Domain.Contracts.Repositories;
using Coffee.Journal.Domain.Entities;
using Moq;

namespace Coffee.Journal.Application.Tests.UseCases;

public class ProfileUseCaseTest
{
    private readonly Mock<IProfileRepository> _repository = new();
    private readonly Mock<ICurrentUser> _currentUser = new();

    private ProfileUseCase CreateUseCase(string? externalId = "ext-1", string? email = "token@example.com")
    {
        _currentUser.SetupGet(x => x.IsAuthenticated).Returns(externalId is not null);
        _currentUser.SetupGet(x => x.ExternalId).Returns(externalId);
        _currentUser.SetupGet(x => x.Email).Returns(email);

        return new ProfileUseCase(_repository.Object, _currentUser.Object);
    }

    [Fact]
    public async Task GivenNewUser_WhenCreateCurrent_ThenTheProfileShouldUseTheTokenIdentity()
    {
        #region Arrange

        ProfileUseCase useCase = CreateUseCase();
        Profile? saved = null;
        _repository.Setup(x => x.AddAsync(It.IsAny<Profile>()))
            .Callback<Profile>(p => saved = p)
            .ReturnsAsync(Guid.NewGuid());

        #endregion

        #region Act

        await useCase.CreateCurrentAsync(CreateRequest("raffael"));

        #endregion

        #region Assert

        Assert.NotNull(saved);
        Assert.Equal("ext-1", saved!.ExternalId);
        Assert.Equal("token@example.com", saved.Email);
        Assert.Equal("raffael", saved.Handle);

        #endregion
    }

    [Fact]
    public async Task GivenTokenWithoutEmail_WhenCreateCurrent_ThenTheRequestEmailShouldBeUsed()
    {
        #region Arrange

        ProfileUseCase useCase = CreateUseCase(email: null);
        Profile? saved = null;
        _repository.Setup(x => x.AddAsync(It.IsAny<Profile>()))
            .Callback<Profile>(p => saved = p)
            .ReturnsAsync(Guid.NewGuid());

        #endregion

        #region Act

        await useCase.CreateCurrentAsync(CreateRequest("raffael", "form@example.com"));

        #endregion

        #region Assert

        Assert.Equal("form@example.com", saved!.Email);

        #endregion
    }

    [Fact]
    public async Task GivenNoEmailAnywhere_WhenCreateCurrent_ThenItShouldFail()
    {
        #region Arrange

        ProfileUseCase useCase = CreateUseCase(email: null);

        #endregion

        #region Act & Assert

        await Assert.ThrowsAsync<RequestValidationException>(() => useCase.CreateCurrentAsync(CreateRequest("raffael")));
        _repository.Verify(x => x.AddAsync(It.IsAny<Profile>()), Times.Never);

        #endregion
    }

    [Fact]
    public async Task GivenUserWithProfile_WhenCreateCurrent_ThenItShouldFail()
    {
        #region Arrange

        ProfileUseCase useCase = CreateUseCase();
        _repository.Setup(x => x.ExternalIdExistsAsync("ext-1", null)).ReturnsAsync(true);

        #endregion

        #region Act & Assert

        await Assert.ThrowsAsync<RequestValidationException>(() => useCase.CreateCurrentAsync(CreateRequest("raffael")));
        _repository.Verify(x => x.AddAsync(It.IsAny<Profile>()), Times.Never);

        #endregion
    }

    [Fact]
    public async Task GivenHandleInUse_WhenCreateCurrent_ThenItShouldFail()
    {
        #region Arrange

        ProfileUseCase useCase = CreateUseCase();
        _repository.Setup(x => x.HandleExistsAsync("taken", null)).ReturnsAsync(true);

        #endregion

        #region Act & Assert

        await Assert.ThrowsAsync<RequestValidationException>(() => useCase.CreateCurrentAsync(CreateRequest("taken")));
        _repository.Verify(x => x.AddAsync(It.IsAny<Profile>()), Times.Never);

        #endregion
    }

    [Fact]
    public async Task GivenEmailInUse_WhenCreateCurrent_ThenItShouldFail()
    {
        #region Arrange

        ProfileUseCase useCase = CreateUseCase();
        _repository.Setup(x => x.EmailExistsAsync("token@example.com", null)).ReturnsAsync(true);

        #endregion

        #region Act & Assert

        await Assert.ThrowsAsync<RequestValidationException>(() => useCase.CreateCurrentAsync(CreateRequest("raffael")));

        #endregion
    }

    [Fact]
    public async Task GivenNoAuthenticatedUser_WhenCreateCurrent_ThenItShouldFail()
    {
        #region Arrange

        ProfileUseCase useCase = CreateUseCase(externalId: null);

        #endregion

        #region Act & Assert

        await Assert.ThrowsAsync<InvalidOperationException>(() => useCase.CreateCurrentAsync(CreateRequest("raffael")));

        #endregion
    }

    [Fact]
    public async Task GivenExistingProfile_WhenGetCurrent_ThenTheRolesShouldBeReturned()
    {
        #region Arrange

        ProfileUseCase useCase = CreateUseCase();
        Profile profile = CreateProfile("ext-1", "raffael");
        profile.Roles.Add(new ProfileRole { Role = new Role { Name = "Contributor", Description = "Contributor" } });
        _repository.Setup(x => x.GetByExternalIdAsync("ext-1")).ReturnsAsync(profile);

        #endregion

        #region Act

        ProfileResponse? response = await useCase.GetCurrentAsync();

        #endregion

        #region Assert

        Assert.NotNull(response);
        Assert.Equal(["Contributor"], response!.Roles);

        #endregion
    }

    [Fact]
    public async Task GivenUserWithoutProfile_WhenGetCurrent_ThenNullShouldBeReturned()
    {
        #region Arrange

        ProfileUseCase useCase = CreateUseCase();

        #endregion

        #region Act

        ProfileResponse? response = await useCase.GetCurrentAsync();

        #endregion

        #region Assert

        Assert.Null(response);

        #endregion
    }

    [Fact]
    public async Task GivenExistingProfile_WhenUpdateCurrent_ThenEditableFieldsShouldChangeButNotTheIdentity()
    {
        #region Arrange

        ProfileUseCase useCase = CreateUseCase();
        Profile profile = CreateProfile("ext-1", "old_handle");
        _repository.Setup(x => x.GetByExternalIdAsync("ext-1")).ReturnsAsync(profile);

        #endregion

        #region Act

        bool updated = await useCase.UpdateCurrentAsync(CreateRequest("new_handle", "other@example.com"));

        #endregion

        #region Assert

        Assert.True(updated);
        Assert.Equal("new_handle", profile.Handle);
        Assert.Equal("ext-1", profile.ExternalId);
        Assert.Equal("old@example.com", profile.Email);
        _repository.Verify(x => x.UpdateAsync(profile), Times.Once);

        #endregion
    }

    [Fact]
    public async Task GivenHandleInUse_WhenUpdateCurrent_ThenItShouldFail()
    {
        #region Arrange

        ProfileUseCase useCase = CreateUseCase();
        Profile profile = CreateProfile("ext-1", "old_handle");
        _repository.Setup(x => x.GetByExternalIdAsync("ext-1")).ReturnsAsync(profile);
        _repository.Setup(x => x.HandleExistsAsync("taken", profile.Id)).ReturnsAsync(true);

        #endregion

        #region Act & Assert

        await Assert.ThrowsAsync<RequestValidationException>(() => useCase.UpdateCurrentAsync(CreateRequest("taken")));
        _repository.Verify(x => x.UpdateAsync(It.IsAny<Profile>()), Times.Never);

        #endregion
    }

    [Fact]
    public async Task GivenUserWithoutProfile_WhenUpdateCurrent_ThenFalseShouldBeReturned()
    {
        #region Arrange

        ProfileUseCase useCase = CreateUseCase();

        #endregion

        #region Act

        bool updated = await useCase.UpdateCurrentAsync(CreateRequest("raffael"));

        #endregion

        #region Assert

        Assert.False(updated);

        #endregion
    }

    [Fact]
    public async Task GivenExistingProfile_WhenDeleteCurrent_ThenItShouldBeDeleted()
    {
        #region Arrange

        ProfileUseCase useCase = CreateUseCase();
        Profile profile = CreateProfile("ext-1", "raffael");
        _repository.Setup(x => x.GetByExternalIdAsync("ext-1")).ReturnsAsync(profile);
        _repository.Setup(x => x.DeleteAsync(profile.Id)).ReturnsAsync(true);

        #endregion

        #region Act

        bool deleted = await useCase.DeleteCurrentAsync();

        #endregion

        #region Assert

        Assert.True(deleted);

        #endregion
    }

    [Fact]
    public async Task GivenUserWithoutProfile_WhenDeleteCurrent_ThenFalseShouldBeReturned()
    {
        #region Arrange

        ProfileUseCase useCase = CreateUseCase();

        #endregion

        #region Act

        bool deleted = await useCase.DeleteCurrentAsync();

        #endregion

        #region Assert

        Assert.False(deleted);

        #endregion
    }

    [Fact]
    public async Task GivenExistingProfile_WhenGetPublicById_ThenTheEmailAndRolesShouldNotBeExposed()
    {
        #region Arrange

        ProfileUseCase useCase = CreateUseCase(externalId: null);
        Profile profile = CreateProfile("ext-1", "raffael");
        _repository.Setup(x => x.GetByIdAsync(profile.Id)).ReturnsAsync(profile);

        #endregion

        #region Act

        PublicProfileResponse? response = await useCase.GetPublicByIdAsync(profile.Id);

        #endregion

        #region Assert

        Assert.NotNull(response);
        Assert.Equal("raffael", response!.Handle);
        Assert.DoesNotContain(typeof(PublicProfileResponse).GetProperties(), p => p.Name is "Email" or "Roles" or "Name");

        #endregion
    }

    private static ProfileRequest CreateRequest(string handle, string? email = null)
    {
        return new ProfileRequest
        {
            Name = "Raffael",
            Handle = handle,
            Email = email,
        };
    }

    private static Profile CreateProfile(string externalId, string handle)
    {
        return new Profile
        {
            Id = Guid.NewGuid(),
            ExternalId = externalId,
            Name = "Raffael",
            Email = "old@example.com",
            Handle = handle,
            LanguagePref = "en",
        };
    }
}
