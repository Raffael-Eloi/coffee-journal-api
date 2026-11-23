using Coffee.Journal.Application.Contracts.Models;
using Coffee.Journal.Domain.Entities;

namespace Coffee.Journal.Application.Mappings;

internal static class CoffeeShopMappings
{
    public static CoffeeShop ToEntity(this CoffeeShopRequest request)
    {
        CoffeeShop shop = new()
        {
            Name = request.Name,
            Website = request.Website,
            CoffeeRoasterId = request.CoffeeRoasterId,
            Rate = request.Rate,
            Review = request.Review,
            Recommend = request.Recommend.GetValueOrDefault(),
            City = request.City,
            State = request.State,
            Country = request.Country,
            Location = request.Location,
        };

        shop.Images = request.ToImages(shop.Id);

        return shop;
    }

    public static void ApplyTo(this CoffeeShopRequest request, CoffeeShop shop)
    {
        shop.Name = request.Name;
        shop.Website = request.Website;
        shop.CoffeeRoasterId = request.CoffeeRoasterId;
        shop.Rate = request.Rate;
        shop.Review = request.Review;
        shop.Recommend = request.Recommend.GetValueOrDefault();
        shop.City = request.City;
        shop.State = request.State;
        shop.Country = request.Country;
        shop.Location = request.Location;

        // The image list in the request replaces the existing one.
        shop.Images.Clear();
        shop.Images.AddRange(request.ToImages(shop.Id));
    }

    public static CoffeeShopResponse ToResponse(this CoffeeShop shop)
    {
        return new CoffeeShopResponse
        {
            Id = shop.Id,
            Name = shop.Name,
            Website = shop.Website,
            CoffeeRoasterId = shop.CoffeeRoasterId,
            Rate = shop.Rate,
            Review = shop.Review,
            Recommend = shop.Recommend,
            City = shop.City,
            State = shop.State,
            Country = shop.Country,
            Location = shop.Location,
            ImageUrls = shop.Images.Select(x => x.Url).ToList(),
            CreatedAt = shop.CreatedAt,
            UpdatedAt = shop.UpdatedAt,
        };
    }

    private static List<CoffeeShopImage> ToImages(this CoffeeShopRequest request, Guid shopId)
    {
        DateTime now = DateTime.UtcNow;

        return request.ImageUrls
            .Select(url => new CoffeeShopImage
            {
                Url = url,
                CoffeeShopId = shopId,
                CreatedAt = now,
                UpdatedAt = now,
            })
            .ToList();
    }
}
