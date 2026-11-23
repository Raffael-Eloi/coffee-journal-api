using Coffee.Journal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coffee.Journal.Infrastructure.Database.Configuration;

internal sealed class CoffeePriceConfiguration : IEntityTypeConfiguration<CoffeePrice>
{
    public void Configure(EntityTypeBuilder<CoffeePrice> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .ValueGeneratedNever();

        builder
            .Property(x => x.Grams)
            .IsRequired();

        builder
            .Property(x => x.Price)
            .HasPrecision(10, 2)
            .IsRequired();

        builder
            .Property(x => x.CreatedAt)
            .IsRequired();

        builder
            .Property(x => x.UpdatedAt)
            .IsRequired();

        builder
            .HasIndex(x => new { x.CoffeeId, x.Grams })
            .IsUnique()
            .HasDatabaseName("UX_CoffeePrices_CoffeeId_Grams");

        builder
            .ToTable("CoffeePrices", t =>
            {
                t.HasCheckConstraint("CK_CoffeePrices_Grams", "[Grams] > 0");
                t.HasCheckConstraint("CK_CoffeePrices_Price", "[Price] > 0");
            });
    }
}
