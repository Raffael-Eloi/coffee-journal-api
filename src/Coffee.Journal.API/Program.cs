using Coffee.Journal.API.Authentication;
using Coffee.Journal.API.Endpoints;
using Coffee.Journal.API.ExceptionHandlers;
using Coffee.Journal.Application.Contracts.UseCases;
using Coffee.Journal.Application.UseCases;
using Coffee.Journal.Infrastructure;
using Coffee.Journal.Infrastructure.Storage;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

const string corsPolicyName = "AllowedWebClient";
const string allowedOriginsConfigKey = "AllowedWebClient";
string[]? allowedOrigins = builder.Configuration.GetSection(allowedOriginsConfigKey).Get<string[]>();
if (allowedOrigins is not { Length: > 0 })
{
    throw new InvalidOperationException($"Configuration '{allowedOriginsConfigKey}' must contain at least one origin.");
}

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: corsPolicyName,
        policy =>
        {
            policy
            .WithOrigins(allowedOrigins)
            .WithHeaders("Content-Type", "Authorization")
            .WithMethods("GET", "POST", "PUT", "DELETE")
            .SetPreflightMaxAge(TimeSpan.FromMinutes(10));
        });
});

builder.Services.AddApiAuthentication(builder.Configuration);

builder.Services.AddValidation();

builder.Services.AddOpenApi();

builder.Services.AddExceptionHandler<RequestValidationExceptionHandler>();
builder.Services.AddExceptionHandler<ForbiddenExceptionHandler>();
builder.Services.AddProblemDetails();

BlobStorageOptions blobStorageOptions = builder.Configuration
    .GetSection(BlobStorageOptions.SectionName)
    .Get<BlobStorageOptions>()
    ?? new BlobStorageOptions();
if (string.IsNullOrWhiteSpace(blobStorageOptions.ConnectionString)
    || string.IsNullOrWhiteSpace(blobStorageOptions.ContainerName))
{
    throw new InvalidOperationException(
        $"Configuration '{BlobStorageOptions.SectionName}' must contain 'ConnectionString' and 'ContainerName'.");
}

builder.Services.AddInfrastructure(
    builder.Configuration.GetConnectionString("CoffeeJournal")
    ?? throw new InvalidOperationException("Connection string 'CoffeeJournal' not found."),
    blobStorageOptions);

builder.Services.AddScoped<IProfileUseCase, ProfileUseCase>();
builder.Services.AddScoped<IProfileRoleUseCase, ProfileRoleUseCase>();
builder.Services.AddScoped<ICoffeeRoasterReviewUseCase, CoffeeRoasterReviewUseCase>();
builder.Services.AddScoped<ICoffeeReviewUseCase, CoffeeReviewUseCase>();
builder.Services.AddScoped<ICoffeeShopReviewUseCase, CoffeeShopReviewUseCase>();
builder.Services.AddScoped<ICoffeeRoasterUseCase, CoffeeRoasterUseCase>();
builder.Services.AddScoped<ICoffeeShopUseCase, CoffeeShopUseCase>();
builder.Services.AddScoped<ICoffeeUseCase, CoffeeUseCase>();
builder.Services.AddScoped<ICoffeeEquipmentUseCase, CoffeeEquipmentUseCase>();
builder.Services.AddScoped<ICoffeeEquipmentCategoryUseCase, CoffeeEquipmentCategoryUseCase>();
builder.Services.AddScoped<IImageUseCase, ImageUseCase>();

var app = builder.Build();

app.Services.ApplyMigrations();

app.UseExceptionHandler();

app.UseCors(corsPolicyName);

app.UseAuthentication();
app.UseAuthorization();

app.MapOpenApi();
app.MapScalarApiReference();

app.UseHttpsRedirection();

MeEndpoint.Map(app);
ProfileEndpoint.Map(app);
ProfileRoleEndpoint.Map(app);
CoffeeRoasterEndpoint.Map(app);
ReviewEndpoint.Map<ICoffeeRoasterReviewUseCase>(app, "coffee-roasters", "Coffee Roaster Reviews");
CoffeeShopEndpoint.Map(app);
ReviewEndpoint.Map<ICoffeeShopReviewUseCase>(app, "coffee-shops", "Coffee Shop Reviews");
CoffeeEndpoint.Map(app);
ReviewEndpoint.Map<ICoffeeReviewUseCase>(app, "coffees", "Coffee Reviews");
CoffeeEquipmentEndpoint.Map(app);
CoffeeEquipmentCategoryEndpoint.Map(app);
ImageEndpoint.Map(app);

app.Run();
