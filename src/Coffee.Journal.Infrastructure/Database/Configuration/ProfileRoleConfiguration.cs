using Coffee.Journal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coffee.Journal.Infrastructure.Database.Configuration;

internal sealed class ProfileRoleConfiguration : IEntityTypeConfiguration<ProfileRole>
{
    public void Configure(EntityTypeBuilder<ProfileRole> builder)
    {
        builder
            .HasKey(x => new { x.RoleId, x.ProfileId });

        builder
            .Property(x => x.CreatedAt)
            .IsRequired();

        builder
            .Property(x => x.UpdatedAt)
            .IsRequired();

        builder
            .HasOne(x => x.Role)
            .WithMany()
            .HasForeignKey(x => x.RoleId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .ToTable("ProfileRoles");
    }
}
