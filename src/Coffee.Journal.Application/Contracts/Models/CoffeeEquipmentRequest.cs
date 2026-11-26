using System.ComponentModel.DataAnnotations;
using Coffee.Journal.Domain.Enums;

namespace Coffee.Journal.Application.Contracts.Models;

public sealed class CoffeeEquipmentRequest : BaseRequest, IValidatableObject
{
    private const int UrlMaxLength = 4000;

    [MaxLength(250)]
    [Required]
    public string Name { get; init; } = string.Empty;

    [MaxLength(250)]
    [Required]
    public string Brand { get; init; } = string.Empty;

    [MaxLength(250)]
    public string? Model { get; init; }

    [MaxLength(4000)]
    [Required]
    public string Review { get; init; } = string.Empty;

    [Range(1, 5)]
    public int Rating { get; init; }

    [Required]
    public bool? Recommend { get; init; }

    // Temporary: once authentication exists, the owner comes from the access token.
    [Required]
    public Guid? ProfileId { get; init; }

    [Required]
    public Guid? EquipmentCategoryId { get; init; }

    [Range(typeof(decimal), "0.01", "99999999.99")]
    public decimal? Price { get; init; }

    public string? Currency { get; init; }

    [MaxLength(2000)]
    public string? Link { get; init; }

    [Required]
    [EnumDataType(typeof(CoffeeEquipmentUseFrequency))]
    public CoffeeEquipmentUseFrequency? FrequencyOfUse { get; init; }

    public string? ImageUrl { get; init; }

    public DateOnly? PurchaseDate { get; init; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Price is null != Currency is null)
        {
            yield return new ValidationResult(
                "Price and currency must be informed together.",
                [nameof(Price), nameof(Currency)]);
        }
        else if (Currency is not null && (Currency.Length != 3 || !Currency.All(char.IsAsciiLetterUpper)))
        {
            yield return new ValidationResult(
                "Currency must be a 3-letter ISO 4217 code in uppercase.",
                [nameof(Currency)]);
        }

        if (Link is not null && !IsHttpUrl(Link))
        {
            yield return new ValidationResult(
                "Link must be a valid http(s) URL.",
                [nameof(Link)]);
        }

        if (ImageUrl is not null && (ImageUrl.Length > UrlMaxLength || !IsHttpUrl(ImageUrl)))
        {
            yield return new ValidationResult(
                $"Image URL must be a valid http(s) URL with at most {UrlMaxLength} characters.",
                [nameof(ImageUrl)]);
        }
    }

    private static bool IsHttpUrl(string value)
    {
        return Uri.TryCreate(value, UriKind.Absolute, out Uri? uri)
            && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
    }
}
