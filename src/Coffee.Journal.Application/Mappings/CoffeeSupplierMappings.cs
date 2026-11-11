using Coffee.Journal.Application.Contracts.Models;
using Coffee.Journal.Domain.Entities;

namespace Coffee.Journal.Application.Mappings;

internal static class CoffeeSupplierMappings
{
    public static CoffeeSupplierResponse ToResponse(this CoffeeSupplier supplier)
    {
        return new CoffeeSupplierResponse
        {
            Id = supplier.Id,
            Name = supplier.Name,
            Website = supplier.Website,
            Rate = supplier.Rate,
            Review = supplier.Review,
            Recommend = supplier.Recommend,
            HasMembershipOption = supplier.HasMembershipOption,
            LogoUrl = supplier.LogoUrl,
            CreatedAt = supplier.CreatedAt,
            UpdatedAt = supplier.UpdatedAt,
        };
    }
}
