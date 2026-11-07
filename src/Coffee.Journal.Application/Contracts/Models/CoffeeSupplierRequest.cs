namespace Coffee.Journal.Application.Contracts.Models;

public class CoffeeSupplierRequest
{
    public required string Name { get; set; }

    public string? Website { get; set; }

    public required int Rate { get; set; }

    public required string Review { get; set; }

    public required bool Recommend { get; set; }

    public required bool HasMembershipOption { get; set; }

    public string? LogoUrl { get; set; }
}
