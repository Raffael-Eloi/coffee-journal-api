using Coffee.Journal.API.Endpoints;
using Coffee.Journal.API.ExceptionHandlers;
using Coffee.Journal.Application.Contracts.UseCases;
using Coffee.Journal.Application.UseCases;
using Coffee.Journal.Infrastructure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidation();

builder.Services.AddOpenApi();

builder.Services.AddExceptionHandler<RequestValidationExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddInfrastructure(
    builder.Configuration.GetConnectionString("CoffeeJournal")
    ?? throw new InvalidOperationException("Connection string 'CoffeeJournal' not found."));

builder.Services.AddScoped<ICoffeeSupplierUseCase, CoffeeSupplierUseCase>();
builder.Services.AddScoped<ICoffeeShopUseCase, CoffeeShopUseCase>();
builder.Services.AddScoped<ICoffeeUseCase, CoffeeUseCase>();

var app = builder.Build();

app.Services.ApplyMigrations();

app.UseExceptionHandler();

app.MapOpenApi();
app.MapScalarApiReference();

app.UseHttpsRedirection();

CoffeeSupplierEndpoint.Map(app);
CoffeeShopEndpoint.Map(app);
CoffeeEndpoint.Map(app);

app.Run();
