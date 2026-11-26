using Coffee.Journal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coffee.Journal.Infrastructure.Database.Configuration;

internal sealed class CoffeeShopBrewingMethodConfiguration : IEntityTypeConfiguration<CoffeeShopBrewingMethod>
{
    public void Configure(EntityTypeBuilder<CoffeeShopBrewingMethod> builder)
    {
        builder
            .HasKey(x => new { x.ShopId, x.BrewingMethod });

        builder
            .Property(x => x.BrewingMethod)
            .HasConversion<int>();

        builder
            .ToTable("CoffeeShopBrewingMethods");
    }
}
