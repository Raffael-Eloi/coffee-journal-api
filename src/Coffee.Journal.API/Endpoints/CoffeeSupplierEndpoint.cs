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
    }
}
