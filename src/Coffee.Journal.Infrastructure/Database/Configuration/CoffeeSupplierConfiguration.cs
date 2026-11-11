using Coffee.Journal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coffee.Journal.Infrastructure.Database.Configuration;

internal sealed class CoffeeSupplierConfiguration : IEntityTypeConfiguration<CoffeeSupplier>
{
    public void Configure(EntityTypeBuilder<CoffeeSupplier> builder)
    {
        builder
            .HasKey(x => x.Id);

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
            .Property(x => x.HasMembershipOption)
            .IsRequired();

        builder
            .Property(x => x.LogoUrl)
            .HasMaxLength(4000);

        builder
            .Property(x => x.CreatedAt)
            .IsRequired();

        builder
            .Property(x => x.UpdatedAt)
            .IsRequired();

        builder
            .HasIndex(x => x.Name)
            .IsUnique()
            .HasDatabaseName("UX_CoffeeSuppliers_Name"); ;

        builder
            .ToTable("CoffeeSuppliers", t => t.HasCheckConstraint("CK_CoffeeSuppliers_Rate", "[Rate] BETWEEN 1 AND 5"));
    }
}
