namespace Coffee.Journal.Application.Contracts;

/// <summary>The user who is making the current request, taken from the access token.</summary>
public interface ICurrentUser
{
    bool IsAuthenticated { get; }

    /// <summary>Identifier of the user in the identity provider. Matches Profile.ExternalId.</summary>
    string? ExternalId { get; }

    string? Email { get; }

    string? Name { get; }
}
