using Coffee.Journal.Application.Contracts.Models;
using Coffee.Journal.Application.Contracts.UseCases;
using Microsoft.AspNetCore.Mvc;

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
            [FromBody] CoffeeSupplierRequest request) =>
        {
            Guid supplierId = await coffeeSupplierUseCase.AddAsync(request);
            return Results.Created("/api/v1.0/coffee-supplier/{id}", new { id = supplierId });
        });
    }
}
