using Coffee.Journal.API.Authentication;
using Coffee.Journal.Application.Contracts.Models;
using Coffee.Journal.Application.Contracts.UseCases;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Coffee.Journal.API.Endpoints;

public sealed class CoffeeEndpoint
{
    public static void Map(WebApplication app)
    {
        RouteGroupBuilder group = app
            .MapGroup("/api/v1.0/coffees")
            .WithTags("Coffees");

        group.MapPost("", async (
            [FromServices] ICoffeeUseCase coffeeUseCase,
            [FromServices] ILogger<CoffeeEndpoint> logger,
            [FromBody] CoffeeRequest request) =>
        {
            logger.LogInformation("Creating coffee {Request}", JsonSerializer.Serialize(request));

            Guid coffeeId = await coffeeUseCase.AddAsync(request);
            logger.LogInformation("Coffee {coffeeId} created", coffeeId);

            return Results.Created($"/api/v1.0/coffees/{coffeeId}", new { id = coffeeId });
        })
            .RequireAuthorization(AuthenticationSettings.ContributorPolicy)
            .WithName("Add Coffee");

        group.MapGet("", async (
            [FromServices] ICoffeeUseCase coffeeUseCase) =>
        {
            IReadOnlyList<CoffeeResponse> coffees = await coffeeUseCase.GetAllAsync();

            return Results.Ok(coffees);
        })
            .WithName("Get Coffees");

        group.MapGet("{id:guid}", async (
            [FromServices] ICoffeeUseCase coffeeUseCase,
            Guid id) =>
        {
            CoffeeResponse? coffee = await coffeeUseCase.GetByIdAsync(id);

            return coffee is null ? Results.NotFound() : Results.Ok(coffee);
        })
            .WithName("Get Coffee By Id");

        group.MapPut("{id:guid}", async (
            [FromServices] ICoffeeUseCase coffeeUseCase,
            [FromServices] ILogger<CoffeeEndpoint> logger,
            Guid id,
            [FromBody] CoffeeRequest request) =>
        {
            logger.LogInformation("Updating coffee {coffeeId} {Request}", id, JsonSerializer.Serialize(request));

            bool updated = await coffeeUseCase.UpdateAsync(id, request);
            if (!updated)
            {
                return Results.NotFound();
            }

            logger.LogInformation("Coffee {coffeeId} updated", id);

            return Results.NoContent();
        })
            .RequireAuthorization(AuthenticationSettings.ContributorPolicy)
            .WithName("Update Coffee");

        group.MapDelete("{id:guid}", async (
            [FromServices] ICoffeeUseCase coffeeUseCase,
            [FromServices] ILogger<CoffeeEndpoint> logger,
            Guid id) =>
        {
            bool deleted = await coffeeUseCase.DeleteAsync(id);
            if (!deleted)
            {
                return Results.NotFound();
            }

            logger.LogInformation("Coffee {coffeeId} deleted", id);

            return Results.NoContent();
        })
            .RequireAuthorization(AuthenticationSettings.ContributorPolicy)
            .WithName("Delete Coffee");
    }
}
