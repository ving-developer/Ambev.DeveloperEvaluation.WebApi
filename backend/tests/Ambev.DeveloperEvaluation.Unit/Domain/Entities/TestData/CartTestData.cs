using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData
{
    /// <summary>
    /// Provides methods for generating test data for Cart entities using Bogus.
    /// </summary>
    public static class CartTestData
    {
        private static readonly Faker<Cart> CartFaker = new Faker<Cart>()
            .CustomInstantiator(f => new Cart(
                customerId: Guid.NewGuid(),
                branchId: Guid.NewGuid(),
                saleNumber: f.Commerce.Ean13()
            ));

        private static readonly Faker<CartItem> CartItemFaker = new Faker<CartItem>()
            .CustomInstantiator(f => new CartItem(
                cartId: Guid.NewGuid(),
                productId: Guid.NewGuid(),
                quantity: f.Random.Int(1, 5),
                unitPrice: f.Random.Decimal(10, 500),
                discountPercentage: 0m
            ));

        /// <summary>
        /// Generates a valid Cart with default values (empty items, Pending status).
        /// </summary>
        public static Cart GenerateValidCart()
        {
            return CartFaker.Generate();
        }

        /// <summary>
        /// Gera um carrinho com status Pending e sem itens.
        /// </summary>
        public static Cart GeneratePendingCart()
        {
            var cart = new Cart(Guid.NewGuid(), Guid.NewGuid(), "CODE000001");

            // Adiciona pelo menos um item
            var productId = Guid.NewGuid();
            cart.AddItem(productId, 1, 10.0m);

            return cart;
        }

        /// <summary>
        /// Generates a Cart with a specified number of items.
        /// </summary>
        /// <param name="itemsCount">Number of items to add to the cart.</param>
        public static Cart GenerateCartWithItems(int itemsCount)
        {
            var cart = CartFaker.Generate();
            for (int i = 0; i < itemsCount; i++)
            {
                var item = CartItemFaker.Generate();
                cart.AddItem(item.ProductId, item.Quantity, item.UnitPrice);
            }
            return cart;
        }

        /// <summary>
        /// Generates a Cart that is already completed.
        /// </summary>
        public static Cart GenerateCompletedCart(int itemsCount = 1)
        {
            var cart = GenerateCartWithItems(itemsCount);
            cart.Complete();
            return cart;
        }

        /// <summary>
        /// Generates a Cart that is canceled.
        /// </summary>
        public static Cart GenerateCanceledCart(string reason = "Test cancellation", int itemsCount = 1)
        {
            var cart = GenerateCartWithItems(itemsCount);
            cart.Cancel(reason);
            return cart;
        }

        /// <summary>
        /// Generates a list of carts for testing multiple entries.
        /// </summary>
        /// <param name="count">Number of carts to generate.</param>
        public static List<Cart> GenerateList(int count)
        {
            var list = new List<Cart>();
            for (int i = 0; i < count; i++)
            {
                list.Add(GenerateValidCart());
            }
            return list;
        }
    }
}
