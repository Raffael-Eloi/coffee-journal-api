using System.Security.Claims;

namespace Coffee.Journal.Application.Contracts;

/// <summary>Reads the user's data from the claims of an access token.</summary>
public sealed record CurrentUserClaims(string? ExternalId, string? Email, string? Name)
{
    // The tokens are read with inbound claim mapping turned off, so the claim names are the short ones.
    public const string ObjectId = "oid";
    public const string Subject = "sub";
    public const string Scope = "scp";

    public static CurrentUserClaims From(ClaimsPrincipal? principal)
    {
        if (principal?.Identity?.IsAuthenticated != true)
        {
            return new CurrentUserClaims(null, null, null);
        }

        string? externalId = Find(principal, ObjectId) ?? Find(principal, Subject);
        string? email = Find(principal, "email") ?? Find(principal, "emails") ?? Find(principal, "preferred_username");
        string? name = Find(principal, "name");

        return new CurrentUserClaims(externalId, email, name);
    }

    public static bool HasScope(ClaimsPrincipal principal, string scope)
    {
        return principal.FindAll(Scope)
            .SelectMany(claim => claim.Value.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            .Contains(scope, StringComparer.Ordinal);
    }

    private static string? Find(ClaimsPrincipal principal, string type)
    {
        string? value = principal.FindFirst(type)?.Value;

        return string.IsNullOrWhiteSpace(value) ? null : value;
    }
}
