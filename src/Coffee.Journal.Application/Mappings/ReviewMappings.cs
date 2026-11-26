using Coffee.Journal.Application.Contracts.Models;
using Coffee.Journal.Domain.Contracts.Repositories;
using Coffee.Journal.Domain.Entities;

namespace Coffee.Journal.Application.Mappings;

internal static class ReviewMappings
{
    public static void ApplyTo(this ReviewRequest request, IReview review)
    {
        review.Rating = request.Rating;
        review.Review = request.Review;
        review.Recommend = request.Recommend.GetValueOrDefault();
    }

    public static ReviewResponse ToResponse(this ReviewWithAuthor review)
    {
        return new ReviewResponse
        {
            Id = review.Id,
            Rating = review.Rating,
            Review = review.Review,
            Recommend = review.Recommend,
            Author = new ReviewAuthorResponse
            {
                Id = review.ProfileId,
                Handle = review.Handle,
                Nickname = review.Nickname,
                PhotoUrl = review.PhotoUrl,
            },
            CreatedAt = review.CreatedAt,
            UpdatedAt = review.UpdatedAt,
        };
    }

    public static ReviewListResponse ToListResponse(this IReadOnlyList<ReviewWithAuthor> reviews)
    {
        return new ReviewListResponse
        {
            AverageRating = reviews.Count == 0
                ? null
                : Math.Round((decimal)reviews.Average(x => x.Rating), 1, MidpointRounding.AwayFromZero),
            ReviewCount = reviews.Count,
            RecommendedCount = reviews.Count(x => x.Recommend),
            Reviews = reviews.Select(x => x.ToResponse()).ToList(),
        };
    }
}
