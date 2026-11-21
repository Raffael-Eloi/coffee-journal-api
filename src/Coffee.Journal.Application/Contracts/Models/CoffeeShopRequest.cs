using System.ComponentModel.DataAnnotations;

namespace Coffee.Journal.Application.Contracts.Models;

public sealed class CoffeeShopRequest : BaseRequest, IValidatableObject
{
    private const int ImageUrlMaxLength = 4000;

    [MaxLength(250)]
    [Required]
    public string Name { get; init; } = string.Empty;

    [MaxLength(500)]
    [Url]
    public string? Website { get; init; }

    public Guid? CoffeeSupplierId { get; init; }

    [Range(1, 5)]
    public int Rate { get; init; }

    [MaxLength(4000)]
    [Required]
    public string Review { get; init; } = string.Empty;

    [Required]
    public bool? Recommend { get; init; }

    [MaxLength(200)]
    [Required]
    public string City { get; init; } = string.Empty;

    [MaxLength(200)]
    [Required]
    public string State { get; init; } = string.Empty;

    [MaxLength(200)]
    [Required]
    public string Country { get; init; } = string.Empty;

    [MaxLength(500)]
    [Required]
    public string Location { get; init; } = string.Empty;

    [Required]
    public List<string> ImageUrls { get; init; } = [];

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (ImageUrls is null)
        {
            yield break;
        }

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
