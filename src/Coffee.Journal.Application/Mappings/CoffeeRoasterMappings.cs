using Coffee.Journal.Application.Contracts.Models;
using Coffee.Journal.Domain.Entities;
using Coffee.Journal.Domain.Enums;

namespace Coffee.Journal.Application.Mappings;

internal static class CoffeeRoasterMappings
{
    public static CoffeeRoaster ToEntity(this CoffeeRoasterRequest request)
    {
        CoffeeRoaster roaster = new()
        {
            Name = request.Name,
            Description = request.Description,
            Website = request.Website,
            InstagramUrl = request.InstagramUrl,
            PhoneNumber = request.PhoneNumber,
            HasMembershipOption = request.HasMembershipOption.GetValueOrDefault(),
            LogoUrl = request.LogoUrl,
            City = request.City,
            State = request.State,
            Country = request.Country,
            Address = request.Address,
        };

        roaster.Classifications = request.ToClassifications(roaster.Id);
        roaster.Images = request.ToImages(roaster.Id);

        return roaster;
    }

    public static void ApplyTo(this CoffeeRoasterRequest request, CoffeeRoaster roaster)
    {
        roaster.Name = request.Name;
        roaster.Description = request.Description;
        roaster.Website = request.Website;
        roaster.InstagramUrl = request.InstagramUrl;
        roaster.PhoneNumber = request.PhoneNumber;
        roaster.HasMembershipOption = request.HasMembershipOption.GetValueOrDefault();
        roaster.LogoUrl = request.LogoUrl;
        roaster.City = request.City;
        roaster.State = request.State;
        roaster.Country = request.Country;
        roaster.Address = request.Address;

        // Classifications use a composite key, so only the differences are applied.
        List<CoffeeClassification> wanted = request.Classifications.Distinct().ToList();

        roaster.Classifications.RemoveAll(x => !wanted.Contains(x.Classification));

        foreach (CoffeeClassification classification in wanted.Except(roaster.Classifications.Select(x => x.Classification)))
        {
            roaster.Classifications.Add(new CoffeeRoasterClassification
            {
                RoasterId = roaster.Id,
                Classification = classification,
            });
        }

        // The image list in the request replaces the existing one.
        roaster.Images.Clear();
        roaster.Images.AddRange(request.ToImages(roaster.Id));
    }

    public static CoffeeRoasterResponse ToResponse(this CoffeeRoaster roaster)
    {
        return new CoffeeRoasterResponse
        {
            Id = roaster.Id,
            Name = roaster.Name,
            Description = roaster.Description,
            Website = roaster.Website,
            InstagramUrl = roaster.InstagramUrl,
            PhoneNumber = roaster.PhoneNumber,
            HasMembershipOption = roaster.HasMembershipOption,
            LogoUrl = roaster.LogoUrl,
            City = roaster.City,
            State = roaster.State,
            Country = roaster.Country,
            Address = roaster.Address,
            Classifications = roaster.Classifications
                .Select(x => x.Classification)
                .Order()
                .ToList(),
            ImageUrls = roaster.Images.Select(x => x.Url).ToList(),
            CreatedAt = roaster.CreatedAt,
            UpdatedAt = roaster.UpdatedAt,
        };
    }

    private static List<CoffeeRoasterClassification> ToClassifications(this CoffeeRoasterRequest request, Guid roasterId)
    {
        return request.Classifications
            .Distinct()
            .Select(classification => new CoffeeRoasterClassification
            {
                RoasterId = roasterId,
                Classification = classification,
            })
            .ToList();
    }

    private static List<CoffeeRoasterImage> ToImages(this CoffeeRoasterRequest request, Guid roasterId)
    {
        DateTime now = DateTime.UtcNow;

        return request.ImageUrls
            .Select(url => new CoffeeRoasterImage
            {
                Url = url,
                RoasterId = roasterId,
                CreatedAt = now,
                UpdatedAt = now,
            })
            .ToList();
    }
}
