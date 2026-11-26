using System.ComponentModel.DataAnnotations;

namespace Coffee.Journal.Application.Contracts.Models;

public sealed class CoffeeShopOpeningHourRequest
{
    [Required]
    [EnumDataType(typeof(DayOfWeek))]
    public DayOfWeek? DayOfWeek { get; init; }

    [Required]
    public TimeOnly? OpensAt { get; init; }

    [Required]
    public TimeOnly? ClosesAt { get; init; }
}
