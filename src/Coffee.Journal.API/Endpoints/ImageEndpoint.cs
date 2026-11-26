using Coffee.Journal.API.Authentication;
using Coffee.Journal.Application.Contracts.Models;
using Coffee.Journal.Application.Contracts.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Coffee.Journal.API.Endpoints;

public sealed class ImageEndpoint
{
    public static void Map(WebApplication app)
    {
        RouteGroupBuilder group = app
            .MapGroup("/api/v1.0/images")
            .WithTags("Images");

        group.MapPost("", async (
            [FromServices] IImageUseCase imageUseCase,
            [FromServices] ILogger<ImageEndpoint> logger,
            IFormFile file,
            CancellationToken cancellationToken) =>
        {
            await using Stream stream = file.OpenReadStream();

            ImageResponse response = await imageUseCase.UploadAsync(
                stream,
                file.ContentType,
                file.Length,
                cancellationToken);

            logger.LogInformation("Image uploaded {Url}", response.Url);

            return Results.Created(response.Url, response);
        })
            .DisableAntiforgery()
            .RequireAuthorization(AuthenticationSettings.ApiScopePolicy)
            .WithName("Upload Image");
    }
}
