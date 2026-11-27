using Coffee.Journal.Application.Contracts;
using Coffee.Journal.Domain.Contracts;

namespace Coffee.Journal.API.Authentication;

internal sealed class HttpContextCurrentUser : ICurrentUser, IAuditUser
{
    private readonly CurrentUserClaims _claims;

    public HttpContextCurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        System.Security.Claims.ClaimsPrincipal? principal = httpContextAccessor.HttpContext?.User;

        IsAuthenticated = principal?.Identity?.IsAuthenticated == true;
        _claims = CurrentUserClaims.From(principal);
    }

    public bool IsAuthenticated { get; }

    public string? ExternalId => _claims.ExternalId;

    public string? Email => _claims.Email;

    public string? Name => _claims.Name;
}
