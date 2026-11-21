using Coffee.Journal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coffee.Journal.Infrastructure.Database.Configuration;

internal sealed class CoffeeShopImageConfiguration : IEntityTypeConfiguration<CoffeeShopImage>
{
    public void Configure(EntityTypeBuilder<CoffeeShopImage> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .ValueGeneratedNever();

        builder
            .Property(x => x.Url)
            .HasMaxLength(4000)
            .IsRequired();

        builder
            .Property(x => x.CreatedAt)
            .IsRequired();

        builder
            .Property(x => x.UpdatedAt)
            .IsRequired();

        builder
            .ToTable("CoffeeShopImages");
    }
}
