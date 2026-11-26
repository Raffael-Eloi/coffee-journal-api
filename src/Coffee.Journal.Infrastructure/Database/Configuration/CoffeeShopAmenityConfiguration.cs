using Coffee.Journal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coffee.Journal.Infrastructure.Database.Configuration;

internal sealed class CoffeeShopAmenityConfiguration : IEntityTypeConfiguration<CoffeeShopAmenity>
{
    public void Configure(EntityTypeBuilder<CoffeeShopAmenity> builder)
    {
        builder
            .HasKey(x => new { x.ShopId, x.Amenity });

        builder
            .Property(x => x.Amenity)
            .HasConversion<int>();

        builder
            .ToTable("CoffeeShopAmenities");
    }
}
