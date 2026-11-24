using Coffee.Journal.Application.Contracts.Models;
using Coffee.Journal.Domain.Entities;

namespace Coffee.Journal.Application.Mappings;

internal static class CoffeeMappings
{
    public static Domain.Entities.Coffee ToEntity(this CoffeeRequest request)
    {
        Domain.Entities.Coffee coffee = new()
        {
            Name = request.Name,
            Description = request.Description,
            RoasterId = request.RoasterId.GetValueOrDefault(),
            Rate = request.Rate,
            Review = request.Review,
            Recommend = request.Recommend.GetValueOrDefault(),
            Origin = request.Origin,
            Process = request.Process,
            RoastLevel = request.RoastLevel,
            Variety = request.Variety,
            TastingNotes = request.TastingNotes,
        };

        coffee.Prices = request.ToPrices(coffee.Id);
        coffee.Images = request.ToImages(coffee.Id);

        return coffee;
    }

    public static void ApplyTo(this CoffeeRequest request, Domain.Entities.Coffee coffee)
    {
        coffee.Name = request.Name;
        coffee.Description = request.Description;
        coffee.RoasterId = request.RoasterId.GetValueOrDefault();
        coffee.Rate = request.Rate;
        coffee.Review = request.Review;
        coffee.Recommend = request.Recommend.GetValueOrDefault();
        coffee.Origin = request.Origin;
        coffee.Process = request.Process;
        coffee.RoastLevel = request.RoastLevel;
        coffee.Variety = request.Variety;
        coffee.TastingNotes = request.TastingNotes;

        // The price and image lists in the request replace the existing ones.
        coffee.Prices.Clear();
        coffee.Prices.AddRange(request.ToPrices(coffee.Id));

        coffee.Images.Clear();
        coffee.Images.AddRange(request.ToImages(coffee.Id));
    }

    public static CoffeeResponse ToResponse(this Domain.Entities.Coffee coffee)
    {
        return new CoffeeResponse
        {
            Id = coffee.Id,
            Name = coffee.Name,
            Description = coffee.Description,
            RoasterId = coffee.RoasterId,
            Rate = coffee.Rate,
            Review = coffee.Review,
            Recommend = coffee.Recommend,
            Origin = coffee.Origin,
            Process = coffee.Process,
            RoastLevel = coffee.RoastLevel,
            Variety = coffee.Variety,
            TastingNotes = coffee.TastingNotes,
            Prices = coffee.Prices
                .OrderBy(x => x.Grams)
                .Select(x => new CoffeePriceResponse { Grams = x.Grams, Price = x.Price, Currency = x.Currency })
                .ToList(),
            ImageUrls = coffee.Images.Select(x => x.Url).ToList(),
            CreatedAt = coffee.CreatedAt,
            UpdatedAt = coffee.UpdatedAt,
        };
    }

    private static List<CoffeePrice> ToPrices(this CoffeeRequest request, Guid coffeeId)
    {
        DateTime now = DateTime.UtcNow;

        return request.Prices
            .Select(price => new CoffeePrice
            {
                Grams = price.Grams,
                Price = price.Price,
                Currency = price.Currency,
                CoffeeId = coffeeId,
                CreatedAt = now,
                UpdatedAt = now,
            })
            .ToList();
    }

    private static List<CoffeeImage> ToImages(this CoffeeRequest request, Guid coffeeId)
    {
        DateTime now = DateTime.UtcNow;

        return request.ImageUrls
            .Select(url => new CoffeeImage
            {
                Url = url,
                CoffeeId = coffeeId,
                CreatedAt = now,
                UpdatedAt = now,
            })
            .ToList();
    }
}
