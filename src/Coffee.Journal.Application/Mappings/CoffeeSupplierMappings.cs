using Coffee.Journal.Application.Contracts.Models;
using Coffee.Journal.Domain.Entities;

namespace Coffee.Journal.Application.Mappings;

internal static class CoffeeSupplierMappings
{
    public static CoffeeSupplier ToEntity(this CoffeeSupplierRequest request)
    {
        return new CoffeeSupplier
        {
            Name = request.Name,
            Website = request.Website,
            Recommend = request.Recommend.GetValueOrDefault(),
            HasMembershipOption = request.HasMembershipOption.GetValueOrDefault(),
            Rate = request.Rate,
            Review = request.Review,
            LogoUrl = request.LogoUrl,
        };
    }

    public static void ApplyTo(this CoffeeSupplierRequest request, CoffeeSupplier supplier)
    {
        supplier.Name = request.Name;
        supplier.Website = request.Website;
        supplier.Recommend = request.Recommend.GetValueOrDefault();
        supplier.HasMembershipOption = request.HasMembershipOption.GetValueOrDefault();
        supplier.Rate = request.Rate;
        supplier.Review = request.Review;
        supplier.LogoUrl = request.LogoUrl;
    }

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
