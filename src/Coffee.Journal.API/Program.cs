using Coffee.Journal.API.Endpoints;
using Coffee.Journal.Application.Contracts.UseCases;
using Coffee.Journal.Application.UseCases;
using Coffee.Journal.Domain.Contracts.Repositories;
using Coffee.Journal.Infrastructure;
using Coffee.Journal.Infrastructure.Repositories;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add DataAnnotation validations
builder.Services.AddValidation();

builder.Services.AddOpenApi();

builder.Services.AddInfrastructure(
    builder.Configuration.GetConnectionString("CoffeeJournal")
    ?? throw new InvalidOperationException("Connection string 'CoffeeJournal' not found."));

builder.Services.AddScoped<ICoffeeSuplierUseCase, CoffeeSuplierUseCase>();

builder.Services.AddScoped<ICoffeeSupplierRepository, CoffeeSupplierRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

CoffeeSupplierEndpoint.Map(app);

app.Run();
