namespace Coffee.Journal.Application.Contracts.Models;

public sealed class ReviewResponse : BaseResponse
{
    public Guid Id { get; init; }

    public int Rating { get; init; }

    public string Review { get; init; } = string.Empty;

    public bool Recommend { get; init; }

    public ReviewAuthorResponse Author { get; init; } = new();

    public DateTime CreatedAt { get; init; }

    public DateTime UpdatedAt { get; init; }
}

/// <summary>The public data of the person who wrote a review.</summary>
public sealed class ReviewAuthorResponse
{
    public Guid Id { get; init; }

    public string Handle { get; init; } = string.Empty;

    public string? Nickname { get; init; }

    public string? PhotoUrl { get; init; }
}

/// <summary>The reviews of a roaster, coffee or coffee shop, with their summary.</summary>
public sealed class ReviewListResponse : BaseResponse
{
    /// <summary>Average rating from 1 to 5 with one decimal, or null when there are no reviews.</summary>
    public decimal? AverageRating { get; init; }

    public int ReviewCount { get; init; }

    public int RecommendedCount { get; init; }

    public IReadOnlyList<ReviewResponse> Reviews { get; init; } = [];
}
