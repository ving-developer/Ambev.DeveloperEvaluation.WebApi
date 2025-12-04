using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Integration.Shared.Constants;
using Ambev.DeveloperEvaluation.Integration.Shared.Fixtures;
using Ambev.DeveloperEvaluation.Integration.Shared.Helpers;
using Ambev.DeveloperEvaluation.Integration.Shared.TestData.Carts;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Products;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.WebApi.Carts
{
    public class CartsControllerTests : IntegrationTestBase
    {
        public CartsControllerTests(IntegrationTestFactory factory) : base(factory) { }

        #region /POST CREATE CART
        [Fact(DisplayName = "CreateCart should return Created when request is valid")]
        public async Task CreateCart_ShouldReturnCreated()
        {
            var controller = CreateController<CartsController>();
            var productsController = CreateController<ProductsController>();

            var product = await ProductTestHelper.CreateTestProduct(productsController);

            var request = CreateCartRequestTestData.GetCreateCartRequestWithItems(
                [product.Id],
                [2]);

            var result = await controller.CreateCart(request, default);

            var createdCart = ExtractCreatedCartResponse(result);
            createdCart.Id.Should().NotBeEmpty();
            createdCart.BranchId.Should().Be(IntegrationTestConstants.InitialBranchId);
            createdCart.CustomerId.Should().Be(IntegrationTestConstants.InitialUserId);
            createdCart.Items.Should().HaveCount(1);
            createdCart.Items.First().ProductId.Should().Be(product.Id);
        }
        #endregion

        #region /GET/{id} GET CART
        [Fact(DisplayName = "GetCart should return cart when exists")]
        public async Task GetCart_ShouldReturnCart()
        {
            var controller = CreateController<CartsController>();
            var productsController = CreateController<ProductsController>();

            var createdCart = await CreateTestCart(controller, productsController);

            var result = await controller.GetCart(createdCart.Id, default);

            var ok = result.Should().BeOfType<OkObjectResult>().Which;
            var response = ok.Value.Should().BeAssignableTo<ApiResponseWithData<CartResponse>>().Which;
            response.Data!.Id.Should().Be(createdCart.Id);
            response.Data.BranchId.Should().Be(IntegrationTestConstants.InitialBranchId);
        }
        #endregion

        #region /PATCH/{id}/cancel
        [Fact(DisplayName = "CancelCart should return Ok when cart exists")]
        public async Task CancelCart_ShouldReturnOk()
        {
            var controller = CreateController<CartsController>();
            var productsController = CreateController<ProductsController>();

            var cart = await CreateTestCart(controller, productsController);
            var request = CancelCartRequestTestData.GetValidCancelCartRequest();

            var result = await controller.CancelCart(cart.Id, request, default);

            var ok = result.Should().BeOfType<OkObjectResult>().Which;
            ok.Value.Should().BeAssignableTo<ApiResponse>().Which.Success.Should().BeTrue();
        }
        #endregion

        #region /POST/{id}/items AddItemToCart
        [Fact(DisplayName = "AddItemToCart should add item successfully")]
        public async Task AddItemToCart_ShouldAddItem()
        {
            var controller = CreateController<CartsController>();
            var productsController = CreateController<ProductsController>();

            var cart = await CreateTestCart(controller, productsController);
            var product = await ProductTestHelper.CreateTestProduct(productsController);

            var request = AddItemToCartRequestTestData.GetValidAddItemToCartRequest();
            request.ProductId = product.Id;

            var result = await controller.AddItemToCart(cart.Id, request, default);

            var ok = result.Should().BeOfType<OkObjectResult>().Which;
            var responseWrapper = ok.Value.Should().BeAssignableTo<ApiResponseWithData<CartResponse>>().Which;
            var response = responseWrapper.Data!;
            response.Items.Should().ContainSingle(i => i.ProductId == product.Id);
        }
        #endregion

        #region /DELETE/{cartId}/items/{itemId} RemoveItemFromCart
        [Fact(DisplayName = "RemoveItemFromCart should remove item successfully")]
        public async Task RemoveItemFromCart_ShouldRemoveItem()
        {
            var controller = CreateController<CartsController>();
            var productsController = CreateController<ProductsController>();
            var cart = await CreateEmptyTestCart(controller);
            var product = await ProductTestHelper.CreateTestProduct(productsController);

            var addItemRequest = AddItemToCartRequestTestData.GetValidAddItemToCartRequest();
            addItemRequest.ProductId = product.Id;

            var addResult = await controller.AddItemToCart(cart.Id, addItemRequest, default);
            var addOkResult = addResult.Should().BeOfType<OkObjectResult>().Which;
            var addResponseWrapper = addOkResult.Value.Should().BeAssignableTo<ApiResponseWithData<CartResponse>>().Which;
            var cartAfterAdd = addResponseWrapper.Data!;

            cartAfterAdd.Items.Should().HaveCount(1);
            var addedItem = cartAfterAdd.Items.First();
            var result = await controller.RemoveItemFromCart(cart.Id, addedItem.Id, default);

            var ok = result.Should().BeOfType<OkObjectResult>().Which;
            var responseWrapper = ok.Value.Should().BeAssignableTo<ApiResponseWithData<CartResponse>>().Which;
            var response = responseWrapper.Data!;
            response.Items.Should().BeEmpty();
        }
        #endregion

        #region /POST/{id}/complete CompleteCart
        [Fact(DisplayName = "CompleteCart should complete cart successfully")]
        public async Task CompleteCart_ShouldCompleteCart()
        {
            var controller = CreateController<CartsController>();
            var productsController = CreateController<ProductsController>();

            var cart = await CreateTestCart(controller, productsController);

            var result = await controller.CompleteCart(cart.Id, default);

            var ok = result.Should().BeOfType<OkObjectResult>().Which;
            var responseWrapper = ok.Value.Should().BeAssignableTo<ApiResponseWithData<CartResponse>>().Which;
            var response = responseWrapper.Data!;
            response.Status.Should().Be(CartStatus.Completed.ToString());
        }
        #endregion

        #region /PUT/{cartId}/items/{itemId}/quantity UpdateItemQuantity
        [Fact(DisplayName = "UpdateItemQuantity should update quantity successfully")]
        public async Task UpdateItemQuantity_ShouldUpdateQuantity()
        {
            var controller = CreateController<CartsController>();
            var productsController = CreateController<ProductsController>();

            var cart = await CreateTestCart(controller, productsController);
            var product = await ProductTestHelper.CreateTestProduct(productsController);

            var addItemRequest = AddItemToCartRequestTestData.GetValidAddItemToCartRequest();
            addItemRequest.ProductId = product.Id;

            var addResult = await controller.AddItemToCart(cart.Id, addItemRequest, default);
            var addOkResult = addResult.Should().BeOfType<OkObjectResult>().Which;
            var addResponseWrapper = addOkResult.Value.Should().BeAssignableTo<ApiResponseWithData<CartResponse>>().Which;
            var cartAfterAdd = addResponseWrapper.Data!;
            var addedItem = cartAfterAdd.Items.First();

            var updateRequest = UpdateItemQuantityRequestTestData.GetValidUpdateItemQuantityRequest(5);

            var result = await controller.UpdateItemQuantity(cart.Id, addedItem.Id, updateRequest, default);
            var ok = result.Should().BeOfType<OkObjectResult>().Which;
            var responseWrapper = ok.Value.Should().BeAssignableTo<ApiResponseWithData<CartResponse>>().Which;
            var response = responseWrapper.Data!;

            response.Items.First().Quantity.Should().Be(5);
            response.Items.First().ProductId.Should().Be(addedItem.ProductId);
        }
        #endregion

        #region PRIVATE METHODS
        private static CartResponse ExtractCreatedCartResponse(IActionResult result)
        {
            var createdObject = result as CreatedAtRouteResult;

            if (createdObject!.Value is ApiResponseWithData<CartResponse> responseWrapper)
            {
                return responseWrapper.Data!;
            }

            if (createdObject.Value is CartResponse directResponse)
            {
                return directResponse;
            }

            throw new InvalidOperationException($"Could not extract CartResponse. Value type: {createdObject.Value?.GetType().Name}");
        }

        private static async Task<CartResponse> CreateTestCart(
            CartsController controller,
            ProductsController productsController)
        {
            var product = await ProductTestHelper.CreateTestProduct(productsController);

            var request = CreateCartRequestTestData.GetCreateCartRequestWithItems(
                new List<Guid> { product.Id },
                new List<int> { 1 });

            var result = await controller.CreateCart(request, default);
            return ExtractCreatedCartResponse(result);
        }

        private static async Task<CartResponse> CreateEmptyTestCart(
            CartsController controller)
        {
            var request = new CreateCartRequest
            {
                BranchId = IntegrationTestConstants.InitialBranchId,
                CustomerId = IntegrationTestConstants.InitialUserId,
                Items = []
            };

            var result = await controller.CreateCart(request, default);
            return ExtractCreatedCartResponse(result);
        }
        #endregion
    }
}