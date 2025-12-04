using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Integration.Shared.Constants;
using Ambev.DeveloperEvaluation.Integration.Shared.TestData.Products;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Products;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.Integration.Shared.Helpers;

public static class ProductTestHelper
{
    /// <summary>
    /// Creates a test product using the ProductsController
    /// </summary>
    public static async Task<ProductResponse> CreateTestProduct(ProductsController controller)
    {
        var request = CreateProductRequestTestData.GetValidCreateProductRequest();
        var result = await controller.CreateProduct(request, CancellationToken.None);
        return ExtractCreatedProductResponse(result);
    }

    /// <summary>
    /// Creates multiple test products
    /// </summary>
    public static async Task<List<ProductResponse>> CreateTestProducts(
        ProductsController controller,
        int count = 3)
    {
        var products = new List<ProductResponse>();

        for (int i = 0; i < count; i++)
        {
            var product = await CreateTestProduct(controller);
            products.Add(product);
        }

        return products;
    }

    /// <summary>
    /// Creates a product using the initial product constants
    /// </summary>
    public static async Task<ProductResponse> CreateInitialProduct(ProductsController controller)
    {
        var request = new CreateProductRequest
        {
            Title = IntegrationTestConstants.InitialProductTitle,
            Price = IntegrationTestConstants.InitialProductPrice,
            Description = IntegrationTestConstants.InitialProductDescription,
            Category = IntegrationTestConstants.InitialProductCategory,
            Image = IntegrationTestConstants.InitialProductImage
        };

        var result = await controller.CreateProduct(request, CancellationToken.None);
        return ExtractCreatedProductResponse(result);
    }

    private static ProductResponse ExtractCreatedProductResponse(IActionResult result)
    {
        if (result is CreatedAtRouteResult createdResult)
        {
            if (createdResult.Value is ApiResponseWithData<ProductResponse> responseWrapper)
            {
                return responseWrapper.Data!;
            }

            throw new DomainException($"Expected ApiResponseWithData<ProductResponse> but got {createdResult.Value?.GetType().Name}");
        }

        throw new DomainException($"Expected CreatedAtRouteResult but got {result?.GetType().Name}");
    }
}