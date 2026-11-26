using Coffee.Journal.Application.Contracts;
using Coffee.Journal.Application.Exceptions;
using Coffee.Journal.Application.UseCases;
using Coffee.Journal.Domain.Contracts.Repositories;
using Coffee.Journal.Domain.Entities;
using Moq;

namespace Coffee.Journal.Application.Tests.UseCases;

public class ProfileRoleUseCaseTest
{
    private readonly Mock<IProfileRepository> _profileRepository = new();
    private readonly Mock<IRoleRepository> _roleRepository = new();
    private readonly Mock<ICurrentUser> _currentUser = new();
    private readonly ProfileRoleUseCase _useCase;
    private readonly Role _contributor = new() { Id = Guid.NewGuid(), Name = RoleNames.Contributor, Description = "c" };
    private readonly Role _admin = new() { Id = Guid.NewGuid(), Name = RoleNames.Admin, Description = "a" };

    public ProfileRoleUseCaseTest()
    {
        _currentUser.SetupGet(x => x.ExternalId).Returns("admin-ext");
        _roleRepository.Setup(x => x.GetByNameAsync(RoleNames.Contributor)).ReturnsAsync(_contributor);
        _roleRepository.Setup(x => x.GetByNameAsync(RoleNames.Admin)).ReturnsAsync(_admin);
        _useCase = new ProfileRoleUseCase(_profileRepository.Object, _roleRepository.Object, _currentUser.Object);
    }

    [Fact]
    public async Task GivenProfileWithoutRole_WhenGrant_ThenTheRoleShouldBeAdded()
    {
        #region Arrange

        Profile profile = CreateProfile("user-ext");
        _profileRepository.Setup(x => x.GetByIdAsync(profile.Id)).ReturnsAsync(profile);

        #endregion

        #region Act

        bool result = await _useCase.GrantAsync(profile.Id, RoleNames.Contributor);

        #endregion

        #region Assert

        Assert.True(result);
        Assert.Single(profile.Roles, x => x.RoleId == _contributor.Id && x.ProfileId == profile.Id);
        _profileRepository.Verify(x => x.UpdateAsync(profile), Times.Once);

        #endregion
    }

    [Fact]
    public async Task GivenProfileWithRole_WhenGrant_ThenNothingShouldChange()
    {
        #region Arrange

        Profile profile = CreateProfile("user-ext");
        profile.Roles.Add(new ProfileRole { ProfileId = profile.Id, RoleId = _contributor.Id });
        _profileRepository.Setup(x => x.GetByIdAsync(profile.Id)).ReturnsAsync(profile);

        #endregion

        #region Act

        bool result = await _useCase.GrantAsync(profile.Id, RoleNames.Contributor);

        #endregion

        #region Assert

        Assert.True(result);
        Assert.Single(profile.Roles);
        _profileRepository.Verify(x => x.UpdateAsync(It.IsAny<Profile>()), Times.Never);

        #endregion
    }

    [Fact]
    public async Task GivenUnknownRole_WhenGrant_ThenItShouldFail()
    {
        #region Arrange

        Profile profile = CreateProfile("user-ext");
        _profileRepository.Setup(x => x.GetByIdAsync(profile.Id)).ReturnsAsync(profile);

        #endregion

        #region Act & Assert

        await Assert.ThrowsAsync<RequestValidationException>(() => _useCase.GrantAsync(profile.Id, "Boss"));

        #endregion
    }

    [Fact]
    public async Task GivenUnknownProfile_WhenGrantOrRevoke_ThenFalseShouldBeReturned()
    {
        #region Act & Assert

        Assert.False(await _useCase.GrantAsync(Guid.NewGuid(), RoleNames.Contributor));
        Assert.False(await _useCase.RevokeAsync(Guid.NewGuid(), RoleNames.Contributor));

        #endregion
    }

    [Fact]
    public async Task GivenProfileWithRole_WhenRevoke_ThenTheRoleShouldBeRemoved()
    {
        #region Arrange

        Profile profile = CreateProfile("user-ext");
        profile.Roles.Add(new ProfileRole { ProfileId = profile.Id, RoleId = _contributor.Id });
        _profileRepository.Setup(x => x.GetByIdAsync(profile.Id)).ReturnsAsync(profile);

        #endregion

        #region Act

        bool result = await _useCase.RevokeAsync(profile.Id, RoleNames.Contributor);

        #endregion

        #region Assert

        Assert.True(result);
        Assert.Empty(profile.Roles);
        _profileRepository.Verify(x => x.UpdateAsync(profile), Times.Once);

        #endregion
    }

    [Fact]
    public async Task GivenOwnProfile_WhenRevokeAdmin_ThenItShouldFail()
    {
        #region Arrange

        Profile profile = CreateProfile("admin-ext");
        profile.Roles.Add(new ProfileRole { ProfileId = profile.Id, RoleId = _admin.Id });
        _profileRepository.Setup(x => x.GetByIdAsync(profile.Id)).ReturnsAsync(profile);

        #endregion

        #region Act & Assert

        await Assert.ThrowsAsync<RequestValidationException>(() => _useCase.RevokeAsync(profile.Id, RoleNames.Admin));
        Assert.Single(profile.Roles);

        #endregion
    }

    [Fact]
    public async Task GivenAnotherProfile_WhenRevokeAdmin_ThenTheRoleShouldBeRemoved()
    {
        #region Arrange

        Profile profile = CreateProfile("other-admin-ext");
        profile.Roles.Add(new ProfileRole { ProfileId = profile.Id, RoleId = _admin.Id });
        _profileRepository.Setup(x => x.GetByIdAsync(profile.Id)).ReturnsAsync(profile);

        #endregion

        #region Act

        bool result = await _useCase.RevokeAsync(profile.Id, RoleNames.Admin);

        #endregion

        #region Assert

        Assert.True(result);
        Assert.Empty(profile.Roles);

        #endregion
    }

    private static Profile CreateProfile(string externalId)
    {
        return new Profile
        {
            Id = Guid.NewGuid(),
            ExternalId = externalId,
            Name = "Name",
            Email = $"{externalId}@example.com",
            Handle = externalId.Replace('-', '_'),
            LanguagePref = "en",
        };
    }
}
