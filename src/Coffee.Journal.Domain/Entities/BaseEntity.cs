namespace Coffee.Journal.Domain.Entities;

public abstract class BaseEntity : IAuditable
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string CreatedBy { get; set; } = string.Empty;

    public string UpdatedBy { get; set; } = string.Empty;
}
