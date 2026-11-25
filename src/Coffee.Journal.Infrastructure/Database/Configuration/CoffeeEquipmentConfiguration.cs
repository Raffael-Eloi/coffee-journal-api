using Coffee.Journal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coffee.Journal.Infrastructure.Database.Configuration;

internal sealed class CoffeeEquipmentConfiguration : IEntityTypeConfiguration<CoffeeEquipment>
{
    public void Configure(EntityTypeBuilder<CoffeeEquipment> builder)
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
            .Property(x => x.Brand)
            .HasMaxLength(250)
            .IsRequired();

        builder
            .Property(x => x.Model)
            .HasMaxLength(250);

        builder
            .Property(x => x.Review)
            .HasMaxLength(4000)
            .IsRequired();

        builder
            .Property(x => x.Rating)
            .IsRequired();

        builder
            .Property(x => x.Recommend)
            .IsRequired();

        builder
            .Property(x => x.Price)
            .HasPrecision(10, 2);

        builder
            .Property(x => x.Currency)
            .HasMaxLength(3)
            .IsFixedLength();

        builder
            .Property(x => x.Link)
            .HasMaxLength(2000);

        builder
            .Property(x => x.FrequencyOfUse)
            .HasConversion<int>()
            .IsRequired();

        builder
            .Property(x => x.ImageUrl)
            .HasMaxLength(4000);

        builder
            .Property(x => x.CreatedAt)
            .IsRequired();

        builder
            .Property(x => x.UpdatedAt)
            .IsRequired();

        builder
            .HasOne<CoffeeEquipmentCategory>()
            .WithMany()
            .HasForeignKey(x => x.EquipmentCategoryId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .ToTable("CoffeeEquipments", t =>
            {
                t.HasCheckConstraint("CK_CoffeeEquipments_Rating", "[Rating] BETWEEN 1 AND 5");
                t.HasCheckConstraint(
                    "CK_CoffeeEquipments_PriceCurrency",
                    "([Price] IS NULL AND [Currency] IS NULL) OR ([Price] IS NOT NULL AND [Currency] IS NOT NULL)");
            });
    }
}
