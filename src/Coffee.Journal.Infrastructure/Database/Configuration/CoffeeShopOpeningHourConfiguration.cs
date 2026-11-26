using Coffee.Journal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coffee.Journal.Infrastructure.Database.Configuration;

internal sealed class CoffeeShopOpeningHourConfiguration : IEntityTypeConfiguration<CoffeeShopOpeningHour>
{
    public void Configure(EntityTypeBuilder<CoffeeShopOpeningHour> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .ValueGeneratedNever();

        builder
            .Property(x => x.DayOfWeek)
            .HasConversion<int>()
            .IsRequired();

        builder
            .Property(x => x.OpensAt)
            .IsRequired();

        builder
            .Property(x => x.ClosesAt)
            .IsRequired();

        builder
            .Property(x => x.CreatedAt)
            .IsRequired();

        builder
            .Property(x => x.UpdatedAt)
            .IsRequired();

        builder
            .HasIndex(x => new { x.ShopId, x.DayOfWeek, x.OpensAt })
            .IsUnique()
            .HasDatabaseName("UX_CoffeeShopOpeningHours_ShopId_DayOfWeek_OpensAt");

        builder
            .ToTable("CoffeeShopOpeningHours", t =>
            {
                t.HasCheckConstraint("CK_CoffeeShopOpeningHours_DayOfWeek", "[DayOfWeek] BETWEEN 0 AND 6");
                t.HasCheckConstraint("CK_CoffeeShopOpeningHours_Hours", "[OpensAt] <> [ClosesAt]");
            });
    }
}
