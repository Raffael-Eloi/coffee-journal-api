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
            .Property(x => x.Description)
            .HasMaxLength(4000);

        builder
            .Property(x => x.Website)
            .HasMaxLength(500);

        builder
            .Property(x => x.InstagramUrl)
            .HasMaxLength(500);

        builder
            .Property(x => x.PhoneNumber)
            .HasMaxLength(30);

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
            .Property(x => x.Address)
            .HasMaxLength(500)
            .IsRequired();

        builder
            .Property(x => x.TimeZone)
            .HasMaxLength(100)
            .IsRequired();

        builder
            .Property(x => x.CreatedAt)
            .IsRequired();

        builder
            .Property(x => x.UpdatedAt)
            .IsRequired();

        builder
            .HasMany(x => x.Images)
            .WithOne()
            .HasForeignKey(x => x.ShopId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(x => x.Roasters)
            .WithOne()
            .HasForeignKey(x => x.ShopId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(x => x.BrewingMethods)
            .WithOne()
            .HasForeignKey(x => x.ShopId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(x => x.Amenities)
            .WithOne()
            .HasForeignKey(x => x.ShopId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(x => x.OpeningHours)
            .WithOne()
            .HasForeignKey(x => x.ShopId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasIndex(x => new { x.Name, x.Address })
            .IsUnique()
            .HasDatabaseName("UX_CoffeeShops_Name_Address");

        builder
            .ToTable("CoffeeShops");
    }
}
