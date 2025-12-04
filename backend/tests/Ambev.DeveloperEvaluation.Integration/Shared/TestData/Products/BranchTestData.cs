using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Integration.Shared.Constants;

namespace Ambev.DeveloperEvaluation.Integration.Shared.TestData.Products;

internal static class ProductTestData
{
    /// <summary>
    /// Generates an initial Product entity with predefined constants for integration tests.
    /// This product has fixed data as specified in IntegrationTestConstants.
    /// </summary>
    public static Product GetInitialProduct()
    {
        return new Product(
            IntegrationTestConstants.InitialProductId,
            IntegrationTestConstants.InitialProductTitle,
            IntegrationTestConstants.InitialProductPrice,
            IntegrationTestConstants.InitialProductDescription,
            IntegrationTestConstants.InitialProductCategory,
            IntegrationTestConstants.InitialProductImage);
    }
}