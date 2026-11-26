using Coffee.Journal.API.Authentication;
using Coffee.Journal.Application.Contracts.Models;
using Coffee.Journal.Application.Contracts.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Coffee.Journal.API.Endpoints;

/// <summary>Maps the reviews of a roaster, a coffee or a coffee shop. They all work the same way.</summary>
public sealed class ReviewEndpoint
{
    public static void Map<TUseCase>(WebApplication app, string route, string tag)
        where TUseCase : notnull, IReviewUseCase
    {
        RouteGroupBuilder group = app
            .MapGroup($"/api/v1.0/{route}/{{targetId:guid}}/reviews")
            .WithTags(tag);

        group.MapGet("", async (
            [FromServices] TUseCase useCase,
            Guid targetId) =>
        {
            ReviewListResponse? reviews = await useCase.GetAllAsync(targetId);

            return reviews is null ? Results.NotFound() : Results.Ok(reviews);
        })
            .WithName($"Get {tag}");

        group.MapPost("", async (
            [FromServices] TUseCase useCase,
            [FromServices] ILogger<ReviewEndpoint> logger,
            Guid targetId,
            [FromBody] ReviewRequest request) =>
        {
            Guid? reviewId = await useCase.AddAsync(targetId, request);
            if (reviewId is null)
            {
                return Results.NotFound();
            }

            logger.LogInformation("Review {reviewId} created in {tag}", reviewId, tag);

            return Results.Created($"/api/v1.0/{route}/{targetId}/reviews", new { id = reviewId });
        })
            .RequireAuthorization(AuthenticationSettings.ApiScopePolicy)
            .WithName($"Add {tag}");

        group.MapPut("{reviewId:guid}", async (
            [FromServices] TUseCase useCase,
            [FromServices] ILogger<ReviewEndpoint> logger,
            Guid targetId,
            Guid reviewId,
            [FromBody] ReviewRequest request) =>
        {
            bool updated = await useCase.UpdateAsync(targetId, reviewId, request);
            if (!updated)
            {
                return Results.NotFound();
            }

            logger.LogInformation("Review {reviewId} updated in {tag}", reviewId, tag);

            return Results.NoContent();
        })
            .RequireAuthorization(AuthenticationSettings.ApiScopePolicy)
            .WithName($"Update {tag}");

        group.MapDelete("{reviewId:guid}", async (
            [FromServices] TUseCase useCase,
            [FromServices] ILogger<ReviewEndpoint> logger,
            Guid targetId,
            Guid reviewId) =>
        {
            bool deleted = await useCase.DeleteAsync(targetId, reviewId);
            if (!deleted)
            {
                return Results.NotFound();
            }

            logger.LogInformation("Review {reviewId} deleted in {tag}", reviewId, tag);

            return Results.NoContent();
        })
            .RequireAuthorization(AuthenticationSettings.ApiScopePolicy)
            .WithName($"Delete {tag}");
    }
}
