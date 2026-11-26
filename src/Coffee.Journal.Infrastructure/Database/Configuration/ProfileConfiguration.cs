using Coffee.Journal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coffee.Journal.Infrastructure.Database.Configuration;

internal sealed class ProfileConfiguration : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .ValueGeneratedNever();

        builder
            .Property(x => x.ExternalId)
            .HasMaxLength(200)
            .IsRequired();

        builder
            .Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder
            .Property(x => x.Nickname)
            .HasMaxLength(100);

        builder
            .Property(x => x.PhotoUrl)
            .HasMaxLength(4000);

        builder
            .Property(x => x.Email)
            .HasMaxLength(320)
            .IsRequired();

        builder
            .Property(x => x.Handle)
            .HasMaxLength(30)
            .IsRequired();

        builder
            .Property(x => x.Bio)
            .HasMaxLength(1000);

        builder
            .Property(x => x.InstagramUrl)
            .HasMaxLength(500);

        builder
            .Property(x => x.FacebookUrl)
            .HasMaxLength(500);

        builder
            .Property(x => x.LinkedInUrl)
            .HasMaxLength(500);

        builder
            .Property(x => x.XUrl)
            .HasMaxLength(500);

        builder
            .Property(x => x.Website)
            .HasMaxLength(500);

        builder
            .Property(x => x.LanguagePref)
            .HasMaxLength(10)
            .IsRequired();

        builder
            .Property(x => x.CreatedAt)
            .IsRequired();

        builder
            .Property(x => x.UpdatedAt)
            .IsRequired();

        builder
            .HasMany(x => x.Roles)
            .WithOne()
            .HasForeignKey(x => x.ProfileId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasIndex(x => x.ExternalId)
            .IsUnique()
            .HasDatabaseName("UX_Profiles_ExternalId");

        builder
            .HasIndex(x => x.Email)
            .IsUnique()
            .HasDatabaseName("UX_Profiles_Email");

        builder
            .HasIndex(x => x.Handle)
            .IsUnique()
            .HasDatabaseName("UX_Profiles_Handle");

        builder
            .ToTable("Profiles");
    }
}
