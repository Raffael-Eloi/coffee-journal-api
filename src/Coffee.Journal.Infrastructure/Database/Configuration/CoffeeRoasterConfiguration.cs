using Coffee.Journal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coffee.Journal.Infrastructure.Database.Configuration;

internal sealed class CoffeeRoasterConfiguration : IEntityTypeConfiguration<CoffeeRoaster>
{
    public void Configure(EntityTypeBuilder<CoffeeRoaster> builder)
    {
        builder
            .HasKey(x => x.Id);

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
            .Property(x => x.HasMembershipOption)
            .IsRequired();

        builder
            .Property(x => x.LogoUrl)
            .HasMaxLength(4000);

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
            .HasMaxLength(500);

        builder
            .Property(x => x.CreatedAt)
            .IsRequired();

        builder
            .Property(x => x.UpdatedAt)
            .IsRequired();

        builder
            .HasMany(x => x.Images)
            .WithOne()
            .HasForeignKey(x => x.RoasterId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(x => x.Classifications)
            .WithOne()
            .HasForeignKey(x => x.RoasterId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasIndex(x => x.Name)
            .IsUnique()
            .HasDatabaseName("UX_CoffeeRoasters_Name");

        builder
            .ToTable("CoffeeRoasters");
    }
}
