namespace Coffee.Journal.Application.Exceptions;

/// <summary>The user is signed in but is not allowed to change this resource.</summary>
public sealed class ForbiddenException : Exception
{
    public ForbiddenException(string message) : base(message)
    {
    }
}
