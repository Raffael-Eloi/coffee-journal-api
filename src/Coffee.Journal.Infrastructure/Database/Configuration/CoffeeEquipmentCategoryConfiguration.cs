using Coffee.Journal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coffee.Journal.Infrastructure.Database.Configuration;

internal sealed class CoffeeEquipmentCategoryConfiguration : IEntityTypeConfiguration<CoffeeEquipmentCategory>
{
    private static readonly DateTime SeedDate = new(2026, 11, 25, 0, 0, 0, DateTimeKind.Utc);

    public void Configure(EntityTypeBuilder<CoffeeEquipmentCategory> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .ValueGeneratedNever();

        builder
            .Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder
            .Property(x => x.Description)
            .HasMaxLength(500)
            .IsRequired();

        builder
            .Property(x => x.CreatedAt)
            .IsRequired();

        builder
            .Property(x => x.UpdatedAt)
            .IsRequired();

        builder
            .HasIndex(x => x.Name)
            .IsUnique()
            .HasDatabaseName("UX_CoffeeEquipmentCategories_Name");

        builder
            .ToTable("CoffeeEquipmentCategories");

        builder.HasData(
            Seed("a1f4c3a0-0001-4b1e-9c11-5d0a6e000001", "Grinder", "Burr and blade grinders, manual or electric."),
            Seed("a1f4c3a0-0002-4b1e-9c11-5d0a6e000002", "PaperFilter", "Paper filters used to brew filter coffee."),
            Seed("a1f4c3a0-0003-4b1e-9c11-5d0a6e000003", "Scale", "Scales used to weigh coffee and water."),
            Seed("a1f4c3a0-0004-4b1e-9c11-5d0a6e000004", "Mug", "Mugs and cups used to drink coffee."),
            Seed("a1f4c3a0-0005-4b1e-9c11-5d0a6e000005", "Kettle", "Kettles used to heat and pour water."),
            Seed("a1f4c3a0-0006-4b1e-9c11-5d0a6e000006", "Brewer", "Brewing devices such as V60, AeroPress or espresso machines."),
            Seed("a1f4c3a0-0007-4b1e-9c11-5d0a6e000007", "Accessory", "Any other coffee accessory."));
    }

    private static object Seed(string id, string name, string description)
    {
        return new
        {
            Id = Guid.Parse(id),
            Name = name,
            Description = description,
            CreatedAt = SeedDate,
            UpdatedAt = SeedDate,
        };
    }
}
