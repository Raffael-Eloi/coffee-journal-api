using Coffee.Journal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coffee.Journal.Infrastructure.Database.Configuration;

internal sealed class CoffeeShopRoasterConfiguration : IEntityTypeConfiguration<CoffeeShopRoaster>
{
    public void Configure(EntityTypeBuilder<CoffeeShopRoaster> builder)
    {
        builder
            .HasKey(x => new { x.RoasterId, x.ShopId });

        builder
            .HasOne<CoffeeRoaster>()
            .WithMany()
            .HasForeignKey(x => x.RoasterId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .ToTable("CoffeeShopRoasters");
    }
}
