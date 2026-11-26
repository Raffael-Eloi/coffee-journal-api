using Coffee.Journal.API.Authentication;
using Coffee.Journal.Application.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Coffee.Journal.API.Endpoints;

public sealed class MeEndpoint
{
    public static void Map(WebApplication app)
    {
        // Shows what the API understood from the access token. Useful to check the sign-in setup.
        app.MapGet("/api/v1.0/me", ([FromServices] ICurrentUser currentUser) =>
            Results.Ok(new
            {
                externalId = currentUser.ExternalId,
                email = currentUser.Email,
                name = currentUser.Name,
            }))
            .RequireAuthorization(AuthenticationSettings.ApiScopePolicy)
            .WithTags("Me")
            .WithName("Get Current User");
    }
}
