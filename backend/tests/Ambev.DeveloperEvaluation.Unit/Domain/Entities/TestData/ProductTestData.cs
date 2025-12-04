using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

/// <summary>
/// Provides methods for generating test data for Product entities using Bogus.
/// Ensures consistency across tests and allows creation of valid/invalid products.
/// </summary>
public static class ProductTestData
{
    private static readonly Faker<Product> ProductFaker = new Faker<Product>()
        .RuleFor(p => p.Title, f => f.Commerce.ProductName())
        .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
        .RuleFor(p => p.Price, f => f.Random.Decimal(1, 1000))
        .RuleFor(p => p.Category, f => f.PickRandom<ProductCategory>())
        .RuleFor(p => p.Image, f => f.Internet.Avatar())
        .RuleFor(p => p.Rating, f => new Rating(f.Random.Decimal(0, 5), f.Random.Int(0, 1000)));

    /// <summary>
    /// Generates a valid Product entity with randomized data.
    /// </summary>
    public static Product GenerateValidProduct()
    {
        return ProductFaker.Generate();
    }

    /// <summary>
    /// Generates a list of valid Product entities with randomized data.
    /// </summary>
    /// <param name="count">The number of products to generate.</param>
    /// <returns>A list of valid Product entities.</returns>
    public static List<Product> GenerateList(int count)
    {
        var products = new List<Product>();
        for (int i = 0; i < count; i++)
        {
            products.Add(GenerateValidProduct());
        }
        return products;
    }
}
