namespace Coffee.Journal.Application.Contracts.Models;

public class CoffeeSupplierResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Website { get; set; }

    public int Rate { get; set; }

    public string Review { get; set; } = string.Empty;

    public bool Recommend { get; set; }

    public bool HasMembershipOption { get; set; }

    public string? LogoUrl { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
