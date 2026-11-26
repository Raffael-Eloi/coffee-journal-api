using Coffee.Journal.Domain.Enums;

namespace Coffee.Journal.Application.Contracts.Models;

public sealed class CoffeeEquipmentResponse : BaseResponse
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string Brand { get; init; } = string.Empty;

    public string? Model { get; init; }

    public string Review { get; init; } = string.Empty;

    public int Rating { get; init; }

    public bool Recommend { get; init; }

    public Guid ProfileId { get; init; }

    public Guid EquipmentCategoryId { get; init; }

    public decimal? Price { get; init; }

    public string? Currency { get; init; }

    public string? Link { get; init; }

    public CoffeeEquipmentUseFrequency FrequencyOfUse { get; init; }

    public string? ImageUrl { get; init; }

    public DateOnly? PurchaseDate { get; init; }

    public DateTime CreatedAt { get; init; }

    public DateTime UpdatedAt { get; init; }
}
