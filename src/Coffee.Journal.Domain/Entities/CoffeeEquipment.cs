using Coffee.Journal.Domain.Enums;

namespace Coffee.Journal.Domain.Entities;

public sealed class CoffeeEquipment : BaseEntity
{
    public required string Name { get; set; }

    public required string Brand { get; set; }

    public string? Model { get; set; }

    public required string Review { get; set; }

    public required int Rating { get; set; }

    public required bool Recommend { get; set; }

    public required Guid ProfileId { get; set; }

    public required Guid EquipmentCategoryId { get; set; }

    public decimal? Price { get; set; }

    public string? Currency { get; set; }

    public string? Link { get; set; }

    public required CoffeeEquipmentUseFrequency FrequencyOfUse { get; set; }

    public string? ImageUrl { get; set; }

    public DateOnly? PurchaseDate { get; set; }
}
