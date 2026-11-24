using System.ComponentModel.DataAnnotations;

namespace Coffee.Journal.Application.Contracts.Models;

public sealed class CoffeePriceRequest
{
    [Range(1, int.MaxValue)]
    public int Grams { get; init; }

    [Range(typeof(decimal), "0.01", "99999999.99")]
    public decimal Price { get; init; }

    [Required]
    [RegularExpression("^[A-Z]{3}$", ErrorMessage = "Currency must be a 3-letter ISO 4217 code in uppercase.")]
    public string Currency { get; init; } = string.Empty;
}
