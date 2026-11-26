using Coffee.Journal.API.Authentication;
using Coffee.Journal.Application.Contracts.Models;
using Coffee.Journal.Application.Contracts.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Coffee.Journal.API.Endpoints;

public sealed class ProfileEndpoint
{
    public static void Map(WebApplication app)
    {
        RouteGroupBuilder group = app
            .MapGroup("/api/v1.0/profiles")
            .WithTags("Profiles");

        // The profile of the logged-in user.
        group.MapGet("me", async (
            [FromServices] IProfileUseCase useCase) =>
        {
            ProfileResponse? profile = await useCase.GetCurrentAsync();

            return profile is null ? Results.NotFound() : Results.Ok(profile);
        })
            .RequireAuthorization(AuthenticationSettings.ApiScopePolicy)
            .WithName("Get My Profile");

        group.MapPost("me", async (
            [FromServices] IProfileUseCase useCase,
            [FromServices] ILogger<ProfileEndpoint> logger,
            [FromBody] ProfileRequest request) =>
        {
            Guid profileId = await useCase.CreateCurrentAsync(request);
            logger.LogInformation("Profile {profileId} created", profileId);

            return Results.Created("/api/v1.0/profiles/me", new { id = profileId });
        })
            .RequireAuthorization(AuthenticationSettings.ApiScopePolicy)
            .WithName("Create My Profile");

        group.MapPut("me", async (
            [FromServices] IProfileUseCase useCase,
            [FromServices] ILogger<ProfileEndpoint> logger,
            [FromBody] ProfileRequest request) =>
        {
            bool updated = await useCase.UpdateCurrentAsync(request);
            if (!updated)
            {
                return Results.NotFound();
            }

            logger.LogInformation("Profile updated");

            return Results.NoContent();
        })
            .RequireAuthorization(AuthenticationSettings.ApiScopePolicy)
            .WithName("Update My Profile");

        group.MapDelete("me", async (
            [FromServices] IProfileUseCase useCase,
            [FromServices] ILogger<ProfileEndpoint> logger) =>
        {
            bool deleted = await useCase.DeleteCurrentAsync();
            if (!deleted)
            {
                return Results.NotFound();
            }

            logger.LogInformation("Profile deleted");

            return Results.NoContent();
        })
            .RequireAuthorization(AuthenticationSettings.ApiScopePolicy)
            .WithName("Delete My Profile");

        // What anyone can see about a profile.
        group.MapGet("{id:guid}", async (
            [FromServices] IProfileUseCase useCase,
            Guid id) =>
        {
            PublicProfileResponse? profile = await useCase.GetPublicByIdAsync(id);

            return profile is null ? Results.NotFound() : Results.Ok(profile);
        })
            .WithName("Get Public Profile");
    }
}
