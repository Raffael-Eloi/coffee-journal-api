using Coffee.Journal.API.Authentication;
using Coffee.Journal.Application.Contracts.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Coffee.Journal.API.Endpoints;

public sealed class ProfileRoleEndpoint
{
    public static void Map(WebApplication app)
    {
        RouteGroupBuilder group = app
            .MapGroup("/api/v1.0/profiles/{id:guid}/roles")
            .WithTags("Profile Roles")
            .RequireAuthorization(AuthenticationSettings.AdminPolicy);

        group.MapPut("{role}", async (
            [FromServices] IProfileRoleUseCase useCase,
            [FromServices] ILogger<ProfileRoleEndpoint> logger,
            Guid id,
            string role) =>
        {
            bool granted = await useCase.GrantAsync(id, role);
            if (!granted)
            {
                return Results.NotFound();
            }

            logger.LogInformation("Role {role} granted to profile {profileId}", role, id);

            return Results.NoContent();
        })
            .WithName("Grant Profile Role");

        group.MapDelete("{role}", async (
            [FromServices] IProfileRoleUseCase useCase,
            [FromServices] ILogger<ProfileRoleEndpoint> logger,
            Guid id,
            string role) =>
        {
            bool revoked = await useCase.RevokeAsync(id, role);
            if (!revoked)
            {
                return Results.NotFound();
            }

            logger.LogInformation("Role {role} revoked from profile {profileId}", role, id);

            return Results.NoContent();
        })
            .WithName("Revoke Profile Role");
    }
}
