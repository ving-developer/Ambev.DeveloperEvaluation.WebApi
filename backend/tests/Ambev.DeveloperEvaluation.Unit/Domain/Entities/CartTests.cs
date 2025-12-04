using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class CartTests
{
    [Fact(DisplayName = "Given valid cart, When created, Then properties are set")]
    public void Given_ValidCart_When_Created_Then_PropertiesAreSet()
    {
        var cart = CartTestData.GenerateValidCart();

        Assert.NotEqual(Guid.Empty, cart.CustomerId);
        Assert.NotEqual(Guid.Empty, cart.BranchId);
        Assert.False(string.IsNullOrWhiteSpace(cart.SaleNumber));
        Assert.Equal(CartStatus.Pending, cart.Status);
        Assert.NotNull(cart.Items);
        Assert.Empty(cart.Items);
    }


    [Fact(DisplayName = "Given pending cart, When adding item within limit, Then item is added and discount applied")]
    public void Given_PendingCart_When_AddingItem_Then_ItemAddedAndDiscountApplied()
    {
        var cart = CartTestData.GeneratePendingCart();
        var productId = Guid.NewGuid();

        cart.AddItem(productId, 4, 100m);

        var item = cart.Items.First(i => i.ProductId == productId);

        Assert.Equal(4, item.Quantity);
        Assert.Equal(100m, item.UnitPrice);
        Assert.Equal(10m, item.DiscountPercentage);
        Assert.Equal(item.Subtotal * 0.9m, item.TotalPrice);
    }

    [Fact(DisplayName = "Given pending cart, When adding item exceeding 20 units, Then should throw")]
    public void Given_PendingCart_When_AddingItem_ExceedingLimit_Then_ShouldThrow()
    {
        var cart = CartTestData.GeneratePendingCart();
        var productId = Guid.NewGuid();

        Assert.Throws<InvalidOperationException>(() => cart.AddItem(productId, 21, 10m));
    }

    [Fact(DisplayName = "Given cart with item, When updating quantity, Then quantity and discount are recalculated")]
    public void Given_CartWithItem_When_UpdatingQuantity_Then_QuantityAndDiscountRecalculated()
    {
        var cart = CartTestData.GeneratePendingCart();
        var item = cart.Items.First();
        var itemId = item.Id;

        cart.UpdateItemQuantity(itemId, 10);

        var updatedItem = cart.Items.First();
        Assert.Equal(10, updatedItem.Quantity);
        Assert.Equal(20m, updatedItem.DiscountPercentage);
    }

    [Fact(DisplayName = "Given cart is completed, When updating item, Then should throw")]
    public void Given_CompletedCart_When_UpdatingItem_Then_ShouldThrow()
    {
        var cart = CartTestData.GenerateCompletedCart();
        var itemId = cart.Items.First().Id;

        Assert.Throws<InvalidOperationException>(() => cart.UpdateItemQuantity(itemId, 5));
    }

    [Fact(DisplayName = "Given cart is canceled, When adding item, Then should throw")]
    public void Given_CanceledCart_When_AddingItem_Then_ShouldThrow()
    {
        var cart = CartTestData.GenerateCanceledCart();
        var productId = Guid.NewGuid();

        Assert.Throws<InvalidOperationException>(() => cart.AddItem(productId, 1, 10m));
    }

    [Fact(DisplayName = "Given pending cart, When removing item, Then item is removed and discount recalculated")]
    public void Given_PendingCart_When_RemovingItem_Then_ItemRemoved()
    {
        var cart = CartTestData.GeneratePendingCart();
        var item = cart.Items.First();
        var productId = item.ProductId;

        cart.RemoveItem(item.Id);

        Assert.DoesNotContain(cart.Items, i => i.Id == item.Id);
        Assert.Equal(0, cart.Items.Count(i => i.ProductId == productId));
    }

    [Fact(DisplayName = "Given pending cart, When completing with items, Then status is completed")]
    public void Given_PendingCart_When_Completing_Then_StatusIsCompleted()
    {
        var cart = CartTestData.GeneratePendingCart();

        cart.Complete();

        Assert.Equal(CartStatus.Completed, cart.Status);
        Assert.NotNull(cart.UpdatedAt);
    }

    [Fact(DisplayName = "Given pending cart, When canceling, Then status is canceled and reason set")]
    public void Given_PendingCart_When_Canceling_Then_StatusIsCanceledAndReasonSet()
    {
        var cart = CartTestData.GeneratePendingCart();
        var reason = "Customer request";

        cart.Cancel(reason);

        Assert.Equal(CartStatus.Canceled, cart.Status);
        Assert.Equal(reason, cart.CancellationReason);
        Assert.NotNull(cart.CanceledAt);
    }

    [Fact(DisplayName = "Given cart with multiple items of same product, When updating one item, Then all discounts recalculated")]
    public void Given_CartWithMultipleItemsOfSameProduct_When_UpdatingOneItem_Then_DiscountsRecalculated()
    {
        // Guiven
        var cart = new Cart(Guid.NewGuid(), Guid.NewGuid(), "SALE001");
        var productId = Guid.NewGuid();

        // When
        cart.AddItem(productId, 2, 50m);
        cart.AddItem(productId, 3, 50m);

        foreach (var i in cart.Items.Where(x => x.ProductId == productId))
        {
            Assert.Equal(10m, i.DiscountPercentage);
        }
        var firstItem = cart.Items.First(i => i.ProductId == productId);
        cart.UpdateItemQuantity(firstItem.Id, 8);

        // Then
        foreach (var i in cart.Items.Where(x => x.ProductId == productId))
        {
            Assert.Equal(20m, i.DiscountPercentage);
        }
    }

    [Fact(DisplayName = "Given cart with items, When calculating totals, Then calculated properties are correct")]
    public void Given_CartWithItems_When_CalculatingTotals_Then_PropertiesAreCorrect()
    {
        // Given
        var cart = new Cart(Guid.NewGuid(), Guid.NewGuid(), "SALE001");
        var productId1 = Guid.NewGuid();
        var productId2 = Guid.NewGuid();

        cart.AddItem(productId1, 2, 50m);
        cart.AddItem(productId1, 3, 50m);
        cart.AddItem(productId2, 1, 100m);

        var product1TotalDiscount = 50m * 5 * 0.1m;
        var product2TotalDiscount = 0m;

        var expectedSubtotal = 50m * 5 + 100m;
        var expectedTotalDiscount = product1TotalDiscount + product2TotalDiscount;
        var expectedTotalItemCount = 5 + 1;
        var expectedUniqueProductIds = new[] { productId1, productId2 };

        // When & Then
        Assert.Equal(expectedSubtotal, cart.Subtotal);
        Assert.Equal(expectedTotalDiscount, cart.TotalDiscount);
        Assert.Equal(expectedTotalItemCount, cart.TotalItemCount);
        Assert.Equal(expectedUniqueProductIds.OrderBy(x => x), cart.UniqueProductIds.OrderBy(x => x));
        Assert.True(cart.ProductHasDiscount(productId1));
        Assert.False(cart.ProductHasDiscount(productId2));
        Assert.Equal(10m, cart.GetProductDiscountPercentage(productId1));
        Assert.Equal(0m, cart.GetProductDiscountPercentage(productId2));
    }

}
