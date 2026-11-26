using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Coffee.Journal.Domain.Enums;

namespace Coffee.Journal.Application.Contracts.Models;

public sealed partial class CoffeeShopRequest : BaseRequest, IValidatableObject
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
    public string Address { get; init; } = string.Empty;

    /// <summary>IANA time zone name, for example America/Sao_Paulo.</summary>
    [MaxLength(100)]
    [Required]
    public string TimeZone { get; init; } = string.Empty;

    [Required]
    public List<Guid> RoasterIds { get; init; } = [];

    [Required]
    public List<CoffeeBrewingMethod> BrewingMethods { get; init; } = [];

    [Required]
    public List<Amenity> Amenities { get; init; } = [];

    [Required]
    public List<CoffeeShopOpeningHourRequest> OpeningHours { get; init; } = [];

    [Required]
    public List<string> ImageUrls { get; init; } = [];

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!string.IsNullOrEmpty(TimeZone) && !TimeZonePattern().IsMatch(TimeZone))
        {
            yield return new ValidationResult(
                "Time zone must be an IANA name, for example America/Sao_Paulo.",
                [nameof(TimeZone)]);
        }

        if (BrewingMethods is not null)
        {
            for (int index = 0; index < BrewingMethods.Count; index++)
            {
                if (!Enum.IsDefined(BrewingMethods[index]))
                {
                    yield return new ValidationResult(
                        "Brewing method must be a valid value.",
                        [$"{nameof(BrewingMethods)}[{index}]"]);
                }
            }
        }

        if (Amenities is not null)
        {
            for (int index = 0; index < Amenities.Count; index++)
            {
                if (!Enum.IsDefined(Amenities[index]))
                {
                    yield return new ValidationResult(
                        "Amenity must be a valid value.",
                        [$"{nameof(Amenities)}[{index}]"]);
                }
            }
        }

        if (OpeningHours is not null)
        {
            for (int index = 0; index < OpeningHours.Count; index++)
            {
                CoffeeShopOpeningHourRequest? hour = OpeningHours[index];

                if (hour is null || hour.DayOfWeek is null || hour.OpensAt is null || hour.ClosesAt is null)
                {
                    yield return new ValidationResult(
                        "Each opening hour must have dayOfWeek, opensAt and closesAt.",
                        [$"{nameof(OpeningHours)}[{index}]"]);
                }
                else if (!Enum.IsDefined(hour.DayOfWeek.Value))
                {
                    yield return new ValidationResult(
                        "Day of week must be a value from 0 (Sunday) to 6 (Saturday).",
                        [$"{nameof(OpeningHours)}[{index}]"]);
                }
                else if (hour.OpensAt == hour.ClosesAt)
                {
                    yield return new ValidationResult(
                        "Closing time must be different from the opening time.",
                        [$"{nameof(OpeningHours)}[{index}]"]);
                }
            }

            bool hasDuplicatedWindow = OpeningHours
                .Where(x => x is { DayOfWeek: not null, OpensAt: not null })
                .GroupBy(x => new { x.DayOfWeek, x.OpensAt })
                .Any(group => group.Count() > 1);

            if (hasDuplicatedWindow)
            {
                yield return new ValidationResult(
                    "The same day and opening time can appear only once.",
                    [nameof(OpeningHours)]);
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

    [GeneratedRegex(@"^[A-Za-z][A-Za-z0-9_+\-]*(/[A-Za-z0-9_+\-]+)*$")]
    private static partial Regex TimeZonePattern();
}
