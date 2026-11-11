using System.ComponentModel.DataAnnotations;

namespace Coffee.Journal.Application.Contracts.Models;

public sealed class CoffeeSupplierRequest
{
    [MaxLength(250)]
    [Required]
    public string Name { get; init; } = string.Empty;

    [MaxLength(500)]
    [Url]
    public string? Website { get; init; }

    [Range(1, 5)]
    public int Rate { get; init; }

    [MaxLength(4000)]
    [Required]
    public string Review { get; init; } = string.Empty;

    [Required]
    public bool? Recommend { get; init; }

    [Required]
    public bool? HasMembershipOption { get; init; }

    [MaxLength(4000)]
    [Url]
    public string? LogoUrl { get; init; }
}
