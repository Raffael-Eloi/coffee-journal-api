using System.ComponentModel.DataAnnotations;

namespace Coffee.Journal.Application.Contracts.Models;

public sealed class CoffeeRequest : BaseRequest, IValidatableObject
{
    private const int ImageUrlMaxLength = 4000;

    [MaxLength(250)]
    [Required]
    public string Name { get; init; } = string.Empty;

    [MaxLength(4000)]
    [Required]
    public string Description { get; init; } = string.Empty;

    [Required]
    public Guid? RoasterId { get; init; }

    [Range(1, 5)]
    public int Rate { get; init; }

    [MaxLength(4000)]
    [Required]
    public string Review { get; init; } = string.Empty;

    [Required]
    public bool? Recommend { get; init; }

    [MaxLength(200)]
    [Required]
    public string Origin { get; init; } = string.Empty;

    [MaxLength(200)]
    [Required]
    public string Process { get; init; } = string.Empty;

    [MaxLength(100)]
    [Required]
    public string RoastLevel { get; init; } = string.Empty;

    [MaxLength(200)]
    [Required]
    public string Variety { get; init; } = string.Empty;

    [MaxLength(1000)]
    [Required]
    public string TastingNotes { get; init; } = string.Empty;

    [Required]
    public List<CoffeePriceRequest> Prices { get; init; } = [];

    [Required]
    public List<string> ImageUrls { get; init; } = [];

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Prices is not null)
        {
            for (int index = 0; index < Prices.Count; index++)
            {
                CoffeePriceRequest? price = Prices[index];

                if (price is null)
                {
                    yield return new ValidationResult(
                        "Each price must be an object with grams and price.",
                        [$"{nameof(Prices)}[{index}]"]);
                }
                else if (price.Grams < 1 || price.Price < 0.01m || price.Price > 99999999.99m)
                {
                    yield return new ValidationResult(
                        "Grams must be at least 1 and price must be between 0.01 and 99999999.99.",
                        [$"{nameof(Prices)}[{index}]"]);
                }
            }

            bool hasDuplicatedGrams = Prices
                .Where(x => x is not null)
                .GroupBy(x => x.Grams)
                .Any(group => group.Count() > 1);

            if (hasDuplicatedGrams)
            {
                yield return new ValidationResult(
                    "Each grams value can appear only once in the price list.",
                    [nameof(Prices)]);
            }
        }

        if (ImageUrls is not null)
        {
            for (int index = 0; index < ImageUrls.Count; index++)
            {
                string? url = ImageUrls[index];

                bool isValid = url is not null
                    && url.Length <= ImageUrlMaxLength
                    && Uri.TryCreate(url, UriKind.Absolute, out Uri? uri)
                    && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);

                if (!isValid)
                {
                    yield return new ValidationResult(
                        $"Each image URL must be a valid http(s) URL with at most {ImageUrlMaxLength} characters.",
                        [$"{nameof(ImageUrls)}[{index}]"]);
                }
            }
        }
    }
}
