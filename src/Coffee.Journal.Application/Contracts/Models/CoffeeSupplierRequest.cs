using System.ComponentModel.DataAnnotations;

namespace Coffee.Journal.Application.Contracts.Models;

public class CoffeeSupplierRequest
{
    [MaxLength(250)]
    [Required]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    [Url]
    public string? Website { get; set; }

    [Range(1, 5)]
    public int Rate { get; set; }

    [MaxLength(4000)]
    [Required]
    public string Review { get; set; } = string.Empty;

    [Required]
    public bool? Recommend { get; set; }

    [Required]
    public bool? HasMembershipOption { get; set; }

    [MaxLength(4000)]
    [Url]
    public string? LogoUrl { get; set; }
}
