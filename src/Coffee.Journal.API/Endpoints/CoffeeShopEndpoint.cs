using Coffee.Journal.Application.Contracts.Models;
using Coffee.Journal.Application.Contracts.UseCases;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Coffee.Journal.API.Endpoints;

public sealed class CoffeeShopEndpoint
{
    public static void Map(WebApplication app)
    {
        RouteGroupBuilder group = app
            .MapGroup("/api/v1.0/coffee-shops")
            .WithTags("Coffee Shops");

        group.MapPost("", async (
            [FromServices] ICoffeeShopUseCase coffeeShopUseCase,
            [FromServices] ILogger<CoffeeShopEndpoint> logger,
            [FromBody] CoffeeShopRequest request) =>
        {
            logger.LogInformation("Creating coffee shop {Request}", JsonSerializer.Serialize(request));

            Guid shopId = await coffeeShopUseCase.AddAsync(request);
            logger.LogInformation("Coffee shop {shopId} created", shopId);

            return Results.Created($"/api/v1.0/coffee-shops/{shopId}", new { id = shopId });
        })
            .WithName("Add Coffee Shop");

        group.MapGet("", async (
            [FromServices] ICoffeeShopUseCase coffeeShopUseCase) =>
        {
            IReadOnlyList<CoffeeShopResponse> shops = await coffeeShopUseCase.GetAllAsync();

            return Results.Ok(shops);
        })
            .WithName("Get Coffee Shops");

        group.MapGet("{id:guid}", async (
            [FromServices] ICoffeeShopUseCase coffeeShopUseCase,
            Guid id) =>
        {
            CoffeeShopResponse? shop = await coffeeShopUseCase.GetByIdAsync(id);

            return shop is null ? Results.NotFound() : Results.Ok(shop);
        })
            .WithName("Get Coffee Shop By Id");

        group.MapPut("{id:guid}", async (
            [FromServices] ICoffeeShopUseCase coffeeShopUseCase,
            [FromServices] ILogger<CoffeeShopEndpoint> logger,
            Guid id,
            [FromBody] CoffeeShopRequest request) =>
        {
            logger.LogInformation("Updating coffee shop {shopId} {Request}", id, JsonSerializer.Serialize(request));

            bool updated = await coffeeShopUseCase.UpdateAsync(id, request);
            if (!updated)
            {
                return Results.NotFound();
            }

            logger.LogInformation("Coffee shop {shopId} updated", id);

            return Results.NoContent();
        })
            .WithName("Update Coffee Shop");

        group.MapDelete("{id:guid}", async (
            [FromServices] ICoffeeShopUseCase coffeeShopUseCase,
            [FromServices] ILogger<CoffeeShopEndpoint> logger,
            Guid id) =>
        {
            bool deleted = await coffeeShopUseCase.DeleteAsync(id);
            if (!deleted)
            {
                return Results.NotFound();
            }

            logger.LogInformation("Coffee shop {shopId} deleted", id);

            return Results.NoContent();
        })
            .WithName("Delete Coffee Shop");
    }
}
