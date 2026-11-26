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

        group.MapPost("", async (
            [FromServices] IProfileUseCase useCase,
            [FromServices] ILogger<ProfileEndpoint> logger,
            [FromBody] ProfileRequest request) =>
        {
            Guid profileId = await useCase.AddAsync(request);
            logger.LogInformation("Profile {profileId} created", profileId);

            return Results.Created($"/api/v1.0/profiles/{profileId}", new { id = profileId });
        })
            .WithName("Add Profile");

        group.MapGet("{id:guid}", async (
            [FromServices] IProfileUseCase useCase,
            Guid id) =>
        {
            ProfileResponse? profile = await useCase.GetByIdAsync(id);

            return profile is null ? Results.NotFound() : Results.Ok(profile);
        })
            .WithName("Get Profile By Id");

        group.MapPut("{id:guid}", async (
            [FromServices] IProfileUseCase useCase,
            [FromServices] ILogger<ProfileEndpoint> logger,
            Guid id,
            [FromBody] ProfileRequest request) =>
        {
            bool updated = await useCase.UpdateAsync(id, request);
            if (!updated)
            {
                return Results.NotFound();
            }

            logger.LogInformation("Profile {profileId} updated", id);

            return Results.NoContent();
        })
            .WithName("Update Profile");

        group.MapDelete("{id:guid}", async (
            [FromServices] IProfileUseCase useCase,
            [FromServices] ILogger<ProfileEndpoint> logger,
            Guid id) =>
        {
            bool deleted = await useCase.DeleteAsync(id);
            if (!deleted)
            {
                return Results.NotFound();
            }

            logger.LogInformation("Profile {profileId} deleted", id);

            return Results.NoContent();
        })
            .WithName("Delete Profile");
    }
}
