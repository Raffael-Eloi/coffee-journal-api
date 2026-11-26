namespace Coffee.Journal.Domain.Entities;

/// <summary>What the reviews of roasters, coffees and coffee shops have in common.</summary>
public interface IReview
{
    int Rating { get; set; }

    string Review { get; set; }

    bool Recommend { get; set; }

    Guid ProfileId { get; set; }
}
