using Coffee.Journal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coffee.Journal.Infrastructure.Database.Configuration;

internal sealed class CoffeeReviewConfiguration : IEntityTypeConfiguration<CoffeeReview>
{
    public void Configure(EntityTypeBuilder<CoffeeReview> builder)
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
            .HasOne<Domain.Entities.Coffee>()
            .WithMany()
            .HasForeignKey(x => x.CoffeeId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasIndex(x => new { x.CoffeeId, x.ProfileId })
            .IsUnique()
            .HasDatabaseName("UX_CoffeeReviews_CoffeeId_ProfileId");

        builder
            .ToTable("CoffeeReviews", t => t.HasCheckConstraint("CK_CoffeeReviews_Rating", "[Rating] BETWEEN 1 AND 5"));
    }
}
