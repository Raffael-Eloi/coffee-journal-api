using Coffee.Journal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coffee.Journal.Infrastructure.Database.Configuration;

internal sealed class CoffeeConfiguration : IEntityTypeConfiguration<Domain.Entities.Coffee>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Coffee> builder)
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
            .HasMaxLength(4000)
            .IsRequired();

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
            .Property(x => x.Origin)
            .HasMaxLength(200)
            .IsRequired();

        builder
            .Property(x => x.Process)
            .HasMaxLength(200)
            .IsRequired();

        builder
            .Property(x => x.RoastLevel)
            .HasMaxLength(100)
            .IsRequired();

        builder
            .Property(x => x.Variety)
            .HasMaxLength(200)
            .IsRequired();

        builder
            .Property(x => x.TastingNotes)
            .HasMaxLength(1000)
            .IsRequired();

        builder
            .Property(x => x.CreatedAt)
            .IsRequired();

        builder
            .Property(x => x.UpdatedAt)
            .IsRequired();

        builder
            .HasOne<CoffeeRoaster>()
            .WithMany()
            .HasForeignKey(x => x.RoasterId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.Prices)
            .WithOne()
            .HasForeignKey(x => x.CoffeeId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(x => x.Images)
            .WithOne()
            .HasForeignKey(x => x.CoffeeId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasIndex(x => new { x.RoasterId, x.Name })
            .IsUnique()
            .HasDatabaseName("UX_Coffees_RoasterId_Name");

        builder
            .ToTable("Coffees", t => t.HasCheckConstraint("CK_Coffees_Rate", "[Rate] BETWEEN 1 AND 5"));
    }
}
