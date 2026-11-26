using Coffee.Journal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coffee.Journal.Infrastructure.Database.Configuration;

internal sealed class CoffeeRoasterReviewConfiguration : IEntityTypeConfiguration<CoffeeRoasterReview>
{
    public void Configure(EntityTypeBuilder<CoffeeRoasterReview> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .ValueGeneratedNever();

        builder
            .Property(x => x.Rating)
            .IsRequired();

        builder
            .Property(x => x.Review)
            .HasMaxLength(4000)
            .IsRequired();

        builder
            .Property(x => x.Recommend)
            .IsRequired();

        builder
            .Property(x => x.ProfileId)
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
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne<Profile>()
            .WithMany()
            .HasForeignKey(x => x.ProfileId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasIndex(x => new { x.RoasterId, x.ProfileId })
            .IsUnique()
            .HasDatabaseName("UX_CoffeeRoasterReviews_RoasterId_ProfileId");

        builder
            .ToTable("CoffeeRoasterReviews", t => t.HasCheckConstraint("CK_CoffeeRoasterReviews_Rating", "[Rating] BETWEEN 1 AND 5"));
    }
}
