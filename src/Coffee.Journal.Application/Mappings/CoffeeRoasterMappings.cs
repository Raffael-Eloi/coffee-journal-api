using Coffee.Journal.Application.Contracts.Models;
using Coffee.Journal.Domain.Entities;

namespace Coffee.Journal.Application.Mappings;

internal static class CoffeeRoasterMappings
{
    public static CoffeeRoaster ToEntity(this CoffeeRoasterRequest request)
    {
        return new CoffeeRoaster
        {
            Name = request.Name,
            Website = request.Website,
            Recommend = request.Recommend.GetValueOrDefault(),
            HasMembershipOption = request.HasMembershipOption.GetValueOrDefault(),
            Rate = request.Rate,
            Review = request.Review,
            LogoUrl = request.LogoUrl,
        };
    }

    public static void ApplyTo(this CoffeeRoasterRequest request, CoffeeRoaster roaster)
    {
        roaster.Name = request.Name;
        roaster.Website = request.Website;
        roaster.Recommend = request.Recommend.GetValueOrDefault();
        roaster.HasMembershipOption = request.HasMembershipOption.GetValueOrDefault();
        roaster.Rate = request.Rate;
        roaster.Review = request.Review;
        roaster.LogoUrl = request.LogoUrl;
    }

    public static CoffeeRoasterResponse ToResponse(this CoffeeRoaster roaster)
    {
        return new CoffeeRoasterResponse
        {
            Id = roaster.Id,
            Name = roaster.Name,
            Website = roaster.Website,
            Rate = roaster.Rate,
            Review = roaster.Review,
            Recommend = roaster.Recommend,
            HasMembershipOption = roaster.HasMembershipOption,
            LogoUrl = roaster.LogoUrl,
            CreatedAt = roaster.CreatedAt,
            UpdatedAt = roaster.UpdatedAt,
        };
    }
}
