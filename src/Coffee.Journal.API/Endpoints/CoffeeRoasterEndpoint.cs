using Coffee.Journal.Application.Contracts.Models;
using Coffee.Journal.Application.Contracts.UseCases;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Coffee.Journal.API.Endpoints;

public sealed class CoffeeRoasterEndpoint
{
    public static void Map(WebApplication app)
    {
        RouteGroupBuilder group = app
            .MapGroup("/api/v1.0/coffee-roasters")
            .WithTags("Coffee Roasters");

        group.MapPost("", async (
            [FromServices] ICoffeeRoasterUseCase coffeeRoasterUseCase,
            [FromServices] ILogger<CoffeeRoasterEndpoint> logger,
            [FromBody] CoffeeRoasterRequest request) =>
        {
            logger.LogInformation("Creating coffee roaster {Request}", JsonSerializer.Serialize(request));

            Guid roasterId = await coffeeRoasterUseCase.AddAsync(request);
            logger.LogInformation("Coffee roaster {roasterId} created", roasterId);

            return Results.Created($"/api/v1.0/coffee-roasters/{roasterId}", new { id = roasterId });
        })
            .WithName("Add Coffee Roaster");

        group.MapGet("", async (
            [FromServices] ICoffeeRoasterUseCase coffeeRoasterUseCase) =>
        {
            IReadOnlyList<CoffeeRoasterResponse> roasters = await coffeeRoasterUseCase.GetAllAsync();

            return Results.Ok(roasters);
        })
            .WithName("Get Coffee Roasters");

        group.MapGet("{id:guid}", async (
            [FromServices] ICoffeeRoasterUseCase coffeeRoasterUseCase,
            Guid id) =>
        {
            CoffeeRoasterResponse? roaster = await coffeeRoasterUseCase.GetByIdAsync(id);

            return roaster is null ? Results.NotFound() : Results.Ok(roaster);
        })
            .WithName("Get Coffee Roaster By Id");

        group.MapPut("{id:guid}", async (
            [FromServices] ICoffeeRoasterUseCase coffeeRoasterUseCase,
            [FromServices] ILogger<CoffeeRoasterEndpoint> logger,
            Guid id,
            [FromBody] CoffeeRoasterRequest request) =>
        {
            logger.LogInformation("Updating coffee roaster {roasterId} {Request}", id, JsonSerializer.Serialize(request));

            bool updated = await coffeeRoasterUseCase.UpdateAsync(id, request);
            if (!updated)
            {
                return Results.NotFound();
            }

            logger.LogInformation("Coffee roaster {roasterId} updated", id);

            return Results.NoContent();
        })
            .WithName("Update Coffee Roaster");

        group.MapDelete("{id:guid}", async (
            [FromServices] ICoffeeRoasterUseCase coffeeRoasterUseCase,
            [FromServices] ILogger<CoffeeRoasterEndpoint> logger,
            Guid id) =>
        {
            bool deleted = await coffeeRoasterUseCase.DeleteAsync(id);
            if (!deleted)
            {
                return Results.NotFound();
            }

            logger.LogInformation("Coffee roaster {roasterId} deleted", id);

            return Results.NoContent();
        })
            .WithName("Delete Coffee Roaster");
    }
}
