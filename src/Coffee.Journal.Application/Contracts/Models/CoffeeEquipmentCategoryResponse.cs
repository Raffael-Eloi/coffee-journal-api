namespace Coffee.Journal.Application.Contracts.Models;

public sealed class CoffeeEquipmentCategoryResponse : BaseResponse
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;
}
