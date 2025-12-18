using Ambev.DeveloperEvaluation.Application.Common.Products;
using Ambev.DeveloperEvaluation.Common.Pagination;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Integration.Shared.Fixtures;
using Ambev.DeveloperEvaluation.Integration.Shared.Helpers;
using Ambev.DeveloperEvaluation.Integration.Shared.TestData.Products;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Products;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProducts;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.WebApi.Products;

/// <summary>
/// Integration tests for <see cref="ProductsController"/> using the real DI pipeline
/// and real mediator handlers.
/// </summary>
public class ProductsControllerTests : IntegrationTestBase
{
    public ProductsControllerTests(IntegrationTestFactory factory)
        : base(factory) { }

    #region /POST CREATE PRODUCT
    [Fact(DisplayName = "CreateProduct should return Created when request is valid")]
    public async Task CreateProduct_ShouldReturnCreated()
    {
        // Given
        var controller = CreateController<ProductsController>();
        var request = CreateProductRequestTestData.GetValidCreateProductRequest();

        // When
        var result = await controller.CreateProduct(request, default);

        // Then
        var createdProduct = ExtractCreatedProductResponse(result);
        createdProduct.Id.Should().NotBeEmpty();
    }
    #endregion

    #region /GET/{id} GET PRODUCT
    [Fact(DisplayName = "GetProduct should return product when product exists")]
    public async Task GetProduct_ShouldReturnProduct()
    {
        // Given
        var controller = CreateController<ProductsController>();
        var createRequest = CreateProductRequestTestData.GetValidCreateProductRequest();

        var createdResult = await controller.CreateProduct(createRequest, default);
        var createdResponse = ExtractCreatedProductResponse(createdResult);

        // When
        var result = await controller.GetProduct(createdResponse.Id, default);

        // Then
        var ok = result.Should()
            .BeOfType<OkObjectResult>()
            .Which;

        var response = ok.Value.Should()
            .BeAssignableTo<ApiResponseWithData<ProductResponse>>()
            .Which;

        response.Data!.Id.Should().Be(createdResponse.Id);
    }

    [Fact(DisplayName = "GetProduct should throw EntityNotFoundException when product does not exist")]
    public async Task GetProduct_ShouldThrowEntityNotFoundException()
    {
        // Given
        var controller = CreateController<ProductsController>();
        var nonExistingProductId = Guid.NewGuid();

        // When & Then
        await FluentActions
            .Invoking(() => controller.GetProduct(nonExistingProductId, default))
            .Should()
            .ThrowAsync<EntityNotFoundException>();
    }
    #endregion

    #region /DELETE/{id} DELETE PRODUCT
    [Fact(DisplayName = "DeleteProduct should return Ok when product exists")]
    public async Task DeleteProduct_ShouldReturnOk()
    {
        // Given
        var controller = CreateController<ProductsController>();
        var createRequest = CreateProductRequestTestData.GetValidCreateProductRequest();

        var createdResult = await controller.CreateProduct(createRequest, default);
        var createdResponse = ExtractCreatedProductResponse(createdResult);

        // When
        var result = await controller.DeleteProduct(createdResponse.Id, default);

        // Then
        var ok = result.Should()
            .BeOfType<OkObjectResult>()
            .Which;

        ok.Value.Should()
            .BeAssignableTo<ApiResponse>()
            .Which.Success.Should().BeTrue();
    }

    [Fact(DisplayName = "DeleteProduct should throw EntityNotFoundException when product does not exist")]
    public async Task DeleteProduct_ShouldThrowEntityNotFoundException()
    {
        // Given
        var controller = CreateController<ProductsController>();
        var nonExistingProductId = Guid.NewGuid();

        // When & Then
        await FluentActions
            .Invoking(() => controller.DeleteProduct(nonExistingProductId, default))
            .Should()
            .ThrowAsync<EntityNotFoundException>();
    }
    #endregion

    #region /GET LIST PRODUCTS
    [Fact(DisplayName = "ListProducts should return paginated list of products")]
    public async Task ListProducts_ShouldReturnPaginatedProducts()
    {
        // Given
        var controller = CreateController<ProductsController>();
        var request = new ListProductsRequest();

        // When
        var result = await controller.ListProducts(request, default);

        // Then
        var ok = result.Should()
            .BeOfType<OkObjectResult>()
            .Which;

        var paginatedResponse = ok.Value.Should()
            .BeAssignableTo<PaginatedResponse<ProductResult>>()
            .Which;

        paginatedResponse.Data.Should().NotBeNull();
    }
    #endregion

    #region PRIVATE METHODS
    private static ProductResponse ExtractCreatedProductResponse(IActionResult result)
    {
        var createdObject = result as CreatedAtRouteResult;
        var responseWrapper = createdObject!.Value as ApiResponseWithData<ProductResponse>;
        return responseWrapper!.Data!;
    }
    #endregion
}
