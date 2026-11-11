using Coffee.Journal.Application.Contracts.Models;
using Coffee.Journal.Application.Contracts.UseCases;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Coffee.Journal.API.Endpoints;

// TODO: Add sealed to classes
public class CoffeeSupplierEndpoint
{
    public static void Map(WebApplication app)
    {
        RouteGroupBuilder group = app
            .MapGroup("/api/v1.0/coffee-suppliers")
            .WithTags("Coffee Suppliers");

        group.MapPost("", async (
            [FromServices] ICoffeeSuplierUseCase coffeeSupplierUseCase,
            [FromServices] ILogger<CoffeeSupplierEndpoint> logger,
            [FromBody] CoffeeSupplierRequest request) =>
        {
            logger.LogInformation("Creating coffee supplier {Request}", JsonSerializer.Serialize(request));

            Guid supplierId = await coffeeSupplierUseCase.AddAsync(request);
            logger.LogInformation("Coffee supplier {supplierId} created", supplierId);

            return Results.Created("/api/v1.0/coffee-supplier/{id}", new { id = supplierId });
        })
            .WithName("Add Coffee Supplier");

        group.MapGet("", async (
            [FromServices] ICoffeeSuplierUseCase coffeeSupplierUseCase) =>
        {
            IReadOnlyList<CoffeeSupplierResponse> suppliers = await coffeeSupplierUseCase.GetAllAsync();

            return Results.Ok(suppliers);
        })
            .WithName("Get Coffee Suppliers");

        group.MapGet("{id:guid}", async (
            [FromServices] ICoffeeSuplierUseCase coffeeSupplierUseCase,
            Guid id) =>
        {
            CoffeeSupplierResponse? supplier = await coffeeSupplierUseCase.GetByIdAsync(id);

            return supplier is null ? Results.NotFound() : Results.Ok(supplier);
        })
            .WithName("Get Coffee Supplier By Id");

        group.MapPut("{id:guid}", async (
            [FromServices] ICoffeeSuplierUseCase coffeeSupplierUseCase,
            [FromServices] ILogger<CoffeeSupplierEndpoint> logger,
            Guid id,
            [FromBody] CoffeeSupplierRequest request) =>
        {
            logger.LogInformation("Updating coffee supplier {supplierId} {Request}", id, JsonSerializer.Serialize(request));

            bool updated = await coffeeSupplierUseCase.UpdateAsync(id, request);
            if (!updated)
            {
                return Results.NotFound();
            }

            logger.LogInformation("Coffee supplier {supplierId} updated", id);

            return Results.NoContent();
        })
            .WithName("Update Coffee Supplier");

        group.MapDelete("{id:guid}", async (
            [FromServices] ICoffeeSuplierUseCase coffeeSupplierUseCase,
            [FromServices] ILogger<CoffeeSupplierEndpoint> logger,
            Guid id) =>
        {
            bool deleted = await coffeeSupplierUseCase.DeleteAsync(id);
            if (!deleted)
            {
                return Results.NotFound();
            }

            logger.LogInformation("Coffee supplier {supplierId} deleted", id);

            return Results.NoContent();
        })
            .WithName("Delete Coffee Supplier");
    }
}
