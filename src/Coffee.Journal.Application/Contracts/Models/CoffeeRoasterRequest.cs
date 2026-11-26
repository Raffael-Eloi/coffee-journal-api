using System.ComponentModel.DataAnnotations;
using Coffee.Journal.Domain.Enums;

namespace Coffee.Journal.Application.Contracts.Models;

public sealed class CoffeeRoasterRequest : BaseRequest, IValidatableObject
{
    private const int ImageUrlMaxLength = 4000;

    [MaxLength(250)]
    [Required]
    public string Name { get; init; } = string.Empty;

    [MaxLength(4000)]
    public string? Description { get; init; }

    [MaxLength(500)]
    [Url]
    public string? Website { get; init; }

    [MaxLength(500)]
    [Url]
    public string? InstagramUrl { get; init; }

    [MaxLength(30)]
    public string? PhoneNumber { get; init; }

    [Required]
    public bool? HasMembershipOption { get; init; }

    [MaxLength(4000)]
    [Url]
    public string? LogoUrl { get; init; }

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
    public string? Address { get; init; }

    [Required]
    public List<CoffeeClassification> Classifications { get; init; } = [];

    [Required]
    public List<string> ImageUrls { get; init; } = [];

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Classifications is not null)
        {
            for (int index = 0; index < Classifications.Count; index++)
            {
                if (!Enum.IsDefined(Classifications[index]))
                {
                    yield return new ValidationResult(
                        "Classification must be a valid value.",
                        [$"{nameof(Classifications)}[{index}]"]);
                }
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
