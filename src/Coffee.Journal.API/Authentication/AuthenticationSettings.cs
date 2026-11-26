namespace Coffee.Journal.API.Authentication;

public sealed class AuthenticationSettings
{
    public const string SectionName = "Authentication";

    /// <summary>Policy that requires a valid token containing the API scope.</summary>
    public const string ApiScopePolicy = "ApiScope";

    /// <summary>Policy for the signed-in users with the Contributor or the Admin role.</summary>
    public const string ContributorPolicy = "Contributor";

    /// <summary>Policy for the signed-in users with the Admin role.</summary>
    public const string AdminPolicy = "Admin";

    /// <summary>Where the signing keys are discovered, for example https://{subdomain}.ciamlogin.com/{tenantId}/v2.0.</summary>
    public string Authority { get; init; } = string.Empty;

    /// <summary>The "iss" value of the tokens. In Entra External ID it uses the tenant ID as host, not the subdomain.</summary>
    public string Issuer { get; init; } = string.Empty;

    /// <summary>Client ID of the API app registration.</summary>
    public string Audience { get; init; } = string.Empty;

    /// <summary>Scope the access token must contain.</summary>
    public string RequiredScope { get; init; } = string.Empty;
}
