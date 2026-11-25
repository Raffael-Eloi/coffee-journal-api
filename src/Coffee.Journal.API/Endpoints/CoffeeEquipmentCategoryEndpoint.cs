using Coffee.Journal.Application.Contracts.Models;
using Coffee.Journal.Application.Contracts.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Coffee.Journal.API.Endpoints;

public sealed class CoffeeEquipmentCategoryEndpoint
{
    public static void Map(WebApplication app)
    {
        RouteGroupBuilder group = app
            .MapGroup("/api/v1.0/coffee-equipment-categories")
            .WithTags("Coffee Equipment Categories");

        group.MapGet("", async (
            [FromServices] ICoffeeEquipmentCategoryUseCase useCase) =>
        {
            IReadOnlyList<CoffeeEquipmentCategoryResponse> categories = await useCase.GetAllAsync();

            return Results.Ok(categories);
        })
            .WithName("Get Coffee Equipment Categories");
    }
}
