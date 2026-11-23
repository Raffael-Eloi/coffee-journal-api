using Coffee.Journal.API.Endpoints;
using Coffee.Journal.API.ExceptionHandlers;
using Coffee.Journal.Application.Contracts.UseCases;
using Coffee.Journal.Application.UseCases;
using Coffee.Journal.Infrastructure;
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

builder.Services.AddValidation();

builder.Services.AddOpenApi();

builder.Services.AddExceptionHandler<RequestValidationExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddInfrastructure(
    builder.Configuration.GetConnectionString("CoffeeJournal")
    ?? throw new InvalidOperationException("Connection string 'CoffeeJournal' not found."));

builder.Services.AddScoped<ICoffeeRoasterUseCase, CoffeeRoasterUseCase>();
builder.Services.AddScoped<ICoffeeShopUseCase, CoffeeShopUseCase>();
builder.Services.AddScoped<ICoffeeUseCase, CoffeeUseCase>();

var app = builder.Build();

app.Services.ApplyMigrations();

app.UseExceptionHandler();

app.UseCors(corsPolicyName);

app.MapOpenApi();
app.MapScalarApiReference();

app.UseHttpsRedirection();

CoffeeRoasterEndpoint.Map(app);
CoffeeShopEndpoint.Map(app);
CoffeeEndpoint.Map(app);

app.Run();
