using Coffee.Journal.API.Endpoints;
using Coffee.Journal.Application.Contracts.UseCases;
using Coffee.Journal.Application.UseCases;
using Coffee.Journal.Infrastructure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add DataAnnotation validations
builder.Services.AddValidation();

builder.Services.AddOpenApi();

builder.Services.AddInfrastructure(
    builder.Configuration.GetConnectionString("CoffeeJournal")
    ?? throw new InvalidOperationException("Connection string 'CoffeeJournal' not found."));

builder.Services.AddScoped<ICoffeeSuplierUseCase, CoffeeSuplierUseCase>();

var app = builder.Build();

app.Services.ApplyMigrations();

app.MapOpenApi();
app.MapScalarApiReference();

app.UseHttpsRedirection();

CoffeeSupplierEndpoint.Map(app);

app.Run();
