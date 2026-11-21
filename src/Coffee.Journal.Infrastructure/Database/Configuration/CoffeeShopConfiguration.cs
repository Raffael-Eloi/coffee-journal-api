using Coffee.Journal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coffee.Journal.Infrastructure.Database.Configuration;

internal sealed class CoffeeShopConfiguration : IEntityTypeConfiguration<CoffeeShop>
{
    public void Configure(EntityTypeBuilder<CoffeeShop> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .ValueGeneratedNever();

        builder
            .Property(x => x.Name)
            .HasMaxLength(250)
            .IsRequired();

        builder
            .Property(x => x.Website)
            .HasMaxLength(500);

        builder
            .Property(x => x.Rate)
            .IsRequired();

        builder
            .Property(x => x.Review)
            .HasMaxLength(4000)
            .IsRequired();

        builder
            .Property(x => x.Recommend)
            .IsRequired();

        builder
            .Property(x => x.City)
            .HasMaxLength(200)
            .IsRequired();

        builder
            .Property(x => x.State)
            .HasMaxLength(200)
            .IsRequired();

        builder
            .Property(x => x.Country)
            .HasMaxLength(200)
            .IsRequired();

        builder
            .Property(x => x.Location)
            .HasMaxLength(500)
            .IsRequired();

        builder
            .Property(x => x.CreatedAt)
            .IsRequired();

        builder
            .Property(x => x.UpdatedAt)
            .IsRequired();

        builder
            .HasOne<CoffeeSupplier>()
            .WithMany()
            .HasForeignKey(x => x.CoffeeSupplierId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);

        builder
            .HasMany(x => x.Images)
            .WithOne()
            .HasForeignKey(x => x.CoffeeShopId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasIndex(x => new { x.Name, x.Location })
            .IsUnique()
            .HasDatabaseName("UX_CoffeeShops_Name_Location");

        builder
            .ToTable("CoffeeShops", t => t.HasCheckConstraint("CK_CoffeeShops_Rate", "[Rate] BETWEEN 1 AND 5"));
    }
}
