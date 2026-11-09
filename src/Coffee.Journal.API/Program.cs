using Coffee.Journal.API.Endpoints;
using Coffee.Journal.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add DataAnnotation validations
builder.Services.AddValidation();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddInfrastructure(
    builder.Configuration.GetConnectionString("CoffeeJournal")
    ?? throw new InvalidOperationException("Connection string 'CoffeeJournal' not found."));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

CoffeeSupplierEndpoint.Map(app);

app.Run();