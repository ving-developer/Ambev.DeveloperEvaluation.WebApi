using Ambev.DeveloperEvaluation.Domain.Entities;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities
{
    public class CartItemTests
    {
        [Fact(DisplayName = "Given valid parameters, When creating CartItem, Then properties should be set")]
        public void Given_ValidParameters_When_CreatingCartItem_Then_PropertiesShouldBeSet()
        {
            // Given
            var cartId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var quantity = 5;
            var unitPrice = 100m;
            var discount = 10m;

            // When
            var item = new CartItem(cartId, productId, quantity, unitPrice, discount);

            // Then
            Assert.Equal(cartId, item.CartId);
            Assert.Equal(productId, item.ProductId);
            Assert.Equal(quantity, item.Quantity);
            Assert.Equal(unitPrice, item.UnitPrice);
            Assert.Equal(discount, item.DiscountPercentage);
            Assert.Equal(quantity * unitPrice, item.Subtotal);
            Assert.Equal(quantity * unitPrice * discount / 100, item.DiscountAmount);
            Assert.Equal(quantity * unitPrice * (1 - discount / 100), item.TotalPrice);
        }

        [Theory(DisplayName = "Given invalid parameters, When creating CartItem, Then should throw ArgumentException")]
        [InlineData(0, 100, 0)]
        [InlineData(5, -10, 0)]
        [InlineData(5, 100, -5)]
        [InlineData(5, 100, 105)]
        public void Given_InvalidParameters_When_CreatingCartItem_Then_ShouldThrow(int quantity, decimal unitPrice, decimal discount)
        {
            // Given
            var cartId = Guid.NewGuid();
            var productId = Guid.NewGuid();

            // When / Then
            Assert.Throws<ArgumentException>(() => new CartItem(cartId, productId, quantity, unitPrice, discount));
        }

        [Fact(DisplayName = "Given empty ProductId, When creating CartItem, Then should throw ArgumentException")]
        public void Given_EmptyProductId_When_CreatingCartItem_Then_ShouldThrow()
        {
            var cartId = Guid.NewGuid();
            var productId = Guid.Empty;

            Assert.Throws<ArgumentException>(() => new CartItem(cartId, productId, 1, 10m, 0));
        }
    }
}
