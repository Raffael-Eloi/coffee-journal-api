using Coffee.Journal.Application.Contracts.Models;
using Coffee.Journal.Domain.Entities;

namespace Coffee.Journal.Application.Mappings;

internal static class CoffeeEquipmentMappings
{
    public static CoffeeEquipment ToEntity(this CoffeeEquipmentRequest request)
    {
        return new CoffeeEquipment
        {
            Name = request.Name,
            Brand = request.Brand,
            Model = request.Model,
            Review = request.Review,
            Rating = request.Rating,
            Recommend = request.Recommend.GetValueOrDefault(),
            EquipmentCategoryId = request.EquipmentCategoryId.GetValueOrDefault(),
            Price = request.Price,
            Currency = request.Currency,
            Link = request.Link,
            FrequencyOfUse = request.FrequencyOfUse.GetValueOrDefault(),
            ImageUrl = request.ImageUrl,
            PurchaseDate = request.PurchaseDate,
        };
    }

    public static void ApplyTo(this CoffeeEquipmentRequest request, CoffeeEquipment equipment)
    {
        equipment.Name = request.Name;
        equipment.Brand = request.Brand;
        equipment.Model = request.Model;
        equipment.Review = request.Review;
        equipment.Rating = request.Rating;
        equipment.Recommend = request.Recommend.GetValueOrDefault();
        equipment.EquipmentCategoryId = request.EquipmentCategoryId.GetValueOrDefault();
        equipment.Price = request.Price;
        equipment.Currency = request.Currency;
        equipment.Link = request.Link;
        equipment.FrequencyOfUse = request.FrequencyOfUse.GetValueOrDefault();
        equipment.ImageUrl = request.ImageUrl;
        equipment.PurchaseDate = request.PurchaseDate;
    }

    public static CoffeeEquipmentResponse ToResponse(this CoffeeEquipment equipment)
    {
        return new CoffeeEquipmentResponse
        {
            Id = equipment.Id,
            Name = equipment.Name,
            Brand = equipment.Brand,
            Model = equipment.Model,
            Review = equipment.Review,
            Rating = equipment.Rating,
            Recommend = equipment.Recommend,
            EquipmentCategoryId = equipment.EquipmentCategoryId,
            Price = equipment.Price,
            Currency = equipment.Currency,
            Link = equipment.Link,
            FrequencyOfUse = equipment.FrequencyOfUse,
            ImageUrl = equipment.ImageUrl,
            PurchaseDate = equipment.PurchaseDate,
            CreatedAt = equipment.CreatedAt,
            UpdatedAt = equipment.UpdatedAt,
        };
    }

    public static CoffeeEquipmentCategoryResponse ToResponse(this CoffeeEquipmentCategory category)
    {
        return new CoffeeEquipmentCategoryResponse
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
        };
    }
}
