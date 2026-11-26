using Coffee.Journal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coffee.Journal.Infrastructure.Database.Configuration;

internal sealed class CoffeeRoasterClassificationConfiguration : IEntityTypeConfiguration<CoffeeRoasterClassification>
{
    public void Configure(EntityTypeBuilder<CoffeeRoasterClassification> builder)
    {
        builder
            .HasKey(x => new { x.RoasterId, x.Classification });

        builder
            .Property(x => x.Classification)
            .HasConversion<int>();

        builder
            .ToTable("CoffeeRoasterClassifications");
    }
}
