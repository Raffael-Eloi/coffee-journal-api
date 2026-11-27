namespace Coffee.Journal.Domain.Contracts;

/// <summary>The user to record in the CreatedBy and UpdatedBy fields when data is saved.</summary>
public interface IAuditUser
{
    /// <summary>Identifier of the user in the identity provider. Matches Profile.ExternalId.</summary>
    string? ExternalId { get; }

    /// <summary>Email taken from the access token. It can be null when the token has no email.</summary>
    string? Email { get; }
}
