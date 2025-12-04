using Ambev.DeveloperEvaluation.WebApi.Features.Carts.AddItemToCart;
using Bogus;

namespace Ambev.DeveloperEvaluation.Integration.Shared.TestData.Carts;

/// <summary>
/// Provides methods for generating test data for AddItemToCartRequest.
/// </summary>
internal static class AddItemToCartRequestTestData
{
    /// <summary>
    /// Generates a valid AddItemToCartRequest with random product and quantity.
    /// </summary>
    public static AddItemToCartRequest GetValidAddItemToCartRequest()
    {
        return new Faker<AddItemToCartRequest>()
            .RuleFor(i => i.ProductId, f => f.Random.Guid())
            .RuleFor(i => i.Quantity, f => f.Random.Int(1, 10))
            .Generate();
    }

    /// <summary>
    /// Generates an invalid AddItemToCartRequest with invalid quantity.
    /// </summary>
    public static AddItemToCartRequest GetInvalidAddItemToCartRequest()
    {
        return new AddItemToCartRequest
        {
            ProductId = Guid.Empty,
            Quantity = 0
        };
    }
}
