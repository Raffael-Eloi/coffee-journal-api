using Coffee.Journal.Application.Contracts.Models;
using Coffee.Journal.Application.Contracts.UseCases;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Coffee.Journal.API.Endpoints;

public sealed class CoffeeEquipmentEndpoint
{
    public static void Map(WebApplication app)
    {
        RouteGroupBuilder group = app
            .MapGroup("/api/v1.0/coffee-equipments")
            .WithTags("Coffee Equipments");

        group.MapPost("", async (
            [FromServices] ICoffeeEquipmentUseCase useCase,
            [FromServices] ILogger<CoffeeEquipmentEndpoint> logger,
            [FromBody] CoffeeEquipmentRequest request) =>
        {
            logger.LogInformation("Creating coffee equipment {Request}", JsonSerializer.Serialize(request));

            Guid equipmentId = await useCase.AddAsync(request);
            logger.LogInformation("Coffee equipment {equipmentId} created", equipmentId);

            return Results.Created($"/api/v1.0/coffee-equipments/{equipmentId}", new { id = equipmentId });
        })
            .WithName("Add Coffee Equipment");

        group.MapGet("", async (
            [FromServices] ICoffeeEquipmentUseCase useCase) =>
        {
            IReadOnlyList<CoffeeEquipmentResponse> equipments = await useCase.GetAllAsync();

            return Results.Ok(equipments);
        })
            .WithName("Get Coffee Equipments");

        group.MapGet("{id:guid}", async (
            [FromServices] ICoffeeEquipmentUseCase useCase,
            Guid id) =>
        {
            CoffeeEquipmentResponse? equipment = await useCase.GetByIdAsync(id);

            return equipment is null ? Results.NotFound() : Results.Ok(equipment);
        })
            .WithName("Get Coffee Equipment By Id");

        group.MapPut("{id:guid}", async (
            [FromServices] ICoffeeEquipmentUseCase useCase,
            [FromServices] ILogger<CoffeeEquipmentEndpoint> logger,
            Guid id,
            [FromBody] CoffeeEquipmentRequest request) =>
        {
            logger.LogInformation("Updating coffee equipment {equipmentId} {Request}", id, JsonSerializer.Serialize(request));

            bool updated = await useCase.UpdateAsync(id, request);
            if (!updated)
            {
                return Results.NotFound();
            }

            logger.LogInformation("Coffee equipment {equipmentId} updated", id);

            return Results.NoContent();
        })
            .WithName("Update Coffee Equipment");

        group.MapDelete("{id:guid}", async (
            [FromServices] ICoffeeEquipmentUseCase useCase,
            [FromServices] ILogger<CoffeeEquipmentEndpoint> logger,
            Guid id) =>
        {
            bool deleted = await useCase.DeleteAsync(id);
            if (!deleted)
            {
                return Results.NotFound();
            }

            logger.LogInformation("Coffee equipment {equipmentId} deleted", id);

            return Results.NoContent();
        })
            .WithName("Delete Coffee Equipment");
    }
}
