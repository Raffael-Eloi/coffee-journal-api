using System.Security.Claims;
using Coffee.Journal.Application.Contracts;

namespace Coffee.Journal.Application.Tests.Contracts;

public class CurrentUserClaimsTest
{
    [Fact]
    public void GivenAuthenticatedPrincipal_WhenFrom_ThenTheObjectIdShouldBeUsedAsExternalId()
    {
        #region Arrange

        ClaimsPrincipal principal = CreatePrincipal(
            ("oid", "object-id"),
            ("sub", "subject"),
            ("email", "raffael@example.com"),
            ("name", "Raffael"));

        #endregion

        #region Act

        CurrentUserClaims claims = CurrentUserClaims.From(principal);

        #endregion

        #region Assert

        Assert.Equal("object-id", claims.ExternalId);
        Assert.Equal("raffael@example.com", claims.Email);
        Assert.Equal("Raffael", claims.Name);

        #endregion
    }

    [Fact]
    public void GivenPrincipalWithoutObjectIdAndEmail_WhenFrom_ThenTheFallbackClaimsShouldBeUsed()
    {
        #region Arrange

        ClaimsPrincipal principal = CreatePrincipal(
            ("sub", "subject"),
            ("preferred_username", "raffael@example.com"));

        #endregion

        #region Act

        CurrentUserClaims claims = CurrentUserClaims.From(principal);

        #endregion

        #region Assert

        Assert.Equal("subject", claims.ExternalId);
        Assert.Equal("raffael@example.com", claims.Email);
        Assert.Null(claims.Name);

        #endregion
    }

    [Fact]
    public void GivenAnonymousPrincipal_WhenFrom_ThenNothingShouldBeReturned()
    {
        #region Arrange

        ClaimsPrincipal principal = new(new ClaimsIdentity());

        #endregion

        #region Act

        CurrentUserClaims claims = CurrentUserClaims.From(principal);

        #endregion

        #region Assert

        Assert.Null(claims.ExternalId);
        Assert.Null(claims.Email);
        Assert.Null(claims.Name);

        #endregion
    }

    [Theory]
    [InlineData("access_as_user", true)]
    [InlineData("read access_as_user write", true)]
    [InlineData("access_as_users", false)]
    [InlineData("", false)]
    public void GivenScopeClaim_WhenHasScope_ThenTheScopeShouldBeFoundOnlyWhenPresent(string scp, bool expected)
    {
        #region Arrange

        ClaimsPrincipal principal = CreatePrincipal(("scp", scp));

        #endregion

        #region Act

        bool hasScope = CurrentUserClaims.HasScope(principal, "access_as_user");

        #endregion

        #region Assert

        Assert.Equal(expected, hasScope);

        #endregion
    }

    private static ClaimsPrincipal CreatePrincipal(params (string Type, string Value)[] claims)
    {
        ClaimsIdentity identity = new(
            claims.Select(x => new Claim(x.Type, x.Value)),
            authenticationType: "Bearer");

        return new ClaimsPrincipal(identity);
    }
}
