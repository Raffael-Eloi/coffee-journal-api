namespace Coffee.Journal.Domain.Entities;

public sealed class Role : BaseEntity
{
    public required string Name { get; set; }

    public required string Description { get; set; }
}
