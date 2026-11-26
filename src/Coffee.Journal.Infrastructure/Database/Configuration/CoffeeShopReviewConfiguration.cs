using Coffee.Journal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coffee.Journal.Infrastructure.Database.Configuration;

internal sealed class CoffeeShopReviewConfiguration : IEntityTypeConfiguration<CoffeeShopReview>
{
    public void Configure(EntityTypeBuilder<CoffeeShopReview> builder)
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
            .HasOne<CoffeeShop>()
            .WithMany()
            .HasForeignKey(x => x.ShopId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasIndex(x => new { x.ShopId, x.ProfileId })
            .IsUnique()
            .HasDatabaseName("UX_CoffeeShopReviews_ShopId_ProfileId");

        builder
            .ToTable("CoffeeShopReviews", t => t.HasCheckConstraint("CK_CoffeeShopReviews_Rating", "[Rating] BETWEEN 1 AND 5"));
    }
}
