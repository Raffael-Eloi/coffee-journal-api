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
            Description = request.Description,
            Website = request.Website,
            InstagramUrl = request.InstagramUrl,
            PhoneNumber = request.PhoneNumber,
            City = request.City,
            State = request.State,
            Country = request.Country,
            Address = request.Address,
            TimeZone = request.TimeZone,
        };

        shop.Roasters = request.RoasterIds
            .Distinct()
            .Select(roasterId => new CoffeeShopRoaster { RoasterId = roasterId, ShopId = shop.Id })
            .ToList();

        shop.BrewingMethods = request.BrewingMethods
            .Distinct()
            .Select(method => new CoffeeShopBrewingMethod { ShopId = shop.Id, BrewingMethod = method })
            .ToList();

        shop.Amenities = request.Amenities
            .Distinct()
            .Select(amenity => new CoffeeShopAmenity { ShopId = shop.Id, Amenity = amenity })
            .ToList();

        shop.OpeningHours = request.ToOpeningHours(shop.Id);
        shop.Images = request.ToImages(shop.Id);

        return shop;
    }

    public static void ApplyTo(this CoffeeShopRequest request, CoffeeShop shop)
    {
        shop.Name = request.Name;
        shop.Description = request.Description;
        shop.Website = request.Website;
        shop.InstagramUrl = request.InstagramUrl;
        shop.PhoneNumber = request.PhoneNumber;
        shop.City = request.City;
        shop.State = request.State;
        shop.Country = request.Country;
        shop.Address = request.Address;
        shop.TimeZone = request.TimeZone;

        // The link tables use composite keys, so only the differences are applied.
        List<Guid> roasterIds = request.RoasterIds.Distinct().ToList();
        shop.Roasters.RemoveAll(x => !roasterIds.Contains(x.RoasterId));
        foreach (Guid roasterId in roasterIds.Except(shop.Roasters.Select(x => x.RoasterId)).ToList())
        {
            shop.Roasters.Add(new CoffeeShopRoaster { RoasterId = roasterId, ShopId = shop.Id });
        }

        List<Domain.Enums.CoffeeBrewingMethod> methods = request.BrewingMethods.Distinct().ToList();
        shop.BrewingMethods.RemoveAll(x => !methods.Contains(x.BrewingMethod));
        foreach (Domain.Enums.CoffeeBrewingMethod method in methods.Except(shop.BrewingMethods.Select(x => x.BrewingMethod)).ToList())
        {
            shop.BrewingMethods.Add(new CoffeeShopBrewingMethod { ShopId = shop.Id, BrewingMethod = method });
        }

        List<Domain.Enums.Amenity> amenities = request.Amenities.Distinct().ToList();
        shop.Amenities.RemoveAll(x => !amenities.Contains(x.Amenity));
        foreach (Domain.Enums.Amenity amenity in amenities.Except(shop.Amenities.Select(x => x.Amenity)).ToList())
        {
            shop.Amenities.Add(new CoffeeShopAmenity { ShopId = shop.Id, Amenity = amenity });
        }

        // The opening hour and image lists in the request replace the existing ones.
        shop.OpeningHours.Clear();
        shop.OpeningHours.AddRange(request.ToOpeningHours(shop.Id));

        shop.Images.Clear();
        shop.Images.AddRange(request.ToImages(shop.Id));
    }

    public static CoffeeShopResponse ToResponse(this CoffeeShop shop)
    {
        return new CoffeeShopResponse
        {
            Id = shop.Id,
            Name = shop.Name,
            Description = shop.Description,
            Website = shop.Website,
            InstagramUrl = shop.InstagramUrl,
            PhoneNumber = shop.PhoneNumber,
            City = shop.City,
            State = shop.State,
            Country = shop.Country,
            Address = shop.Address,
            TimeZone = shop.TimeZone,
            RoasterIds = shop.Roasters.Select(x => x.RoasterId).ToList(),
            BrewingMethods = shop.BrewingMethods.Select(x => x.BrewingMethod).Order().ToList(),
            Amenities = shop.Amenities.Select(x => x.Amenity).Order().ToList(),
            OpeningHours = shop.OpeningHours
                .OrderBy(x => x.DayOfWeek)
                .ThenBy(x => x.OpensAt)
                .Select(x => new CoffeeShopOpeningHourResponse
                {
                    DayOfWeek = x.DayOfWeek,
                    OpensAt = x.OpensAt,
                    ClosesAt = x.ClosesAt,
                })
                .ToList(),
            ImageUrls = shop.Images.Select(x => x.Url).ToList(),
            CreatedAt = shop.CreatedAt,
            UpdatedAt = shop.UpdatedAt,
        };
    }

    private static List<CoffeeShopOpeningHour> ToOpeningHours(this CoffeeShopRequest request, Guid shopId)
    {
        DateTime now = DateTime.UtcNow;

        return request.OpeningHours
            .Select(hour => new CoffeeShopOpeningHour
            {
                ShopId = shopId,
                DayOfWeek = hour.DayOfWeek.GetValueOrDefault(),
                OpensAt = hour.OpensAt.GetValueOrDefault(),
                ClosesAt = hour.ClosesAt.GetValueOrDefault(),
                CreatedAt = now,
                UpdatedAt = now,
            })
            .ToList();
    }

    private static List<CoffeeShopImage> ToImages(this CoffeeShopRequest request, Guid shopId)
    {
        DateTime now = DateTime.UtcNow;

        return request.ImageUrls
            .Select(url => new CoffeeShopImage
            {
                Url = url,
                ShopId = shopId,
                CreatedAt = now,
                UpdatedAt = now,
            })
            .ToList();
    }
}
