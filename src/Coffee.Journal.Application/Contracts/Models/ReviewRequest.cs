using System.ComponentModel.DataAnnotations;

namespace Coffee.Journal.Application.Contracts.Models;

public sealed class ReviewRequest : BaseRequest
{
    [Range(1, 5)]
    public int Rating { get; init; }

    [MaxLength(4000)]
    [Required]
    public string Review { get; init; } = string.Empty;

    [Required]
    public bool? Recommend { get; init; }
}
