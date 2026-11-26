using Coffee.Journal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coffee.Journal.Infrastructure.Database.Configuration;

internal sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    private static readonly DateTime SeedDate = new(2026, 11, 26, 0, 0, 0, DateTimeKind.Utc);

    public void Configure(EntityTypeBuilder<Role> builder)
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
            .HasDatabaseName("UX_Roles_Name");

        builder
            .ToTable("Roles");

        builder.HasData(
            new
            {
                Id = Guid.Parse("b2e5d4b1-0001-4c2f-8d22-6e1b7f000001"),
                Name = "Admin",
                Description = "Everything, including delete and managing roles.",
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate,
            },
            new
            {
                Id = Guid.Parse("b2e5d4b1-0002-4c2f-8d22-6e1b7f000002"),
                Name = "Contributor",
                Description = "Can create and update roasters, shops and coffees.",
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate,
            });
    }
}
