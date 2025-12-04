using Ambev.DeveloperEvaluation.Integration.Shared.Constants;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart;
using Bogus;

namespace Ambev.DeveloperEvaluation.Integration.Shared.TestData.Carts
{
    internal static class CreateCartRequestTestData
    {
        /// <summary>
        /// Generates a valid CreateCartRequest with empty items list.
        /// Products should be added separately using AddItemToCart endpoint.
        /// </summary>
        public static CreateCartRequest GetValidCreateCartRequest()
        {
            return new Faker<CreateCartRequest>()
                .RuleFor(c => c.BranchId, _ => IntegrationTestConstants.InitialBranchId)
                .RuleFor(c => c.CustomerId, _ => IntegrationTestConstants.InitialUserId)
                .RuleFor(c => c.Items, _ => [])
                .Generate();
        }

        /// <summary>
        /// Generates a CreateCartRequest with predefined product items.
        /// </summary>
        /// <param name="productIds">List of product IDs to add to the cart</param>
        /// <param name="quantities">Optional quantities for each product</param>
        public static CreateCartRequest GetCreateCartRequestWithItems(
            List<Guid> productIds,
            List<int>? quantities = null)
        {
            var items = new List<CartItem>();

            for (int i = 0; i < productIds.Count; i++)
            {
                var quantity = quantities?.Count > i ? quantities[i] : 1;

                items.Add(new CartItem
                {
                    ProductId = productIds[i],
                    Quantity = quantity
                });
            }

            return new CreateCartRequest
            {
                BranchId = IntegrationTestConstants.InitialBranchId,
                CustomerId = IntegrationTestConstants.InitialUserId,
                Items = items
            };
        }

        /// <summary>
        /// Generates an invalid CreateCartRequest with empty fields
        /// </summary>
        public static CreateCartRequest GetInvalidCreateCartRequest()
        {
            return new CreateCartRequest
            {
                BranchId = Guid.Empty,
                CustomerId = Guid.Empty,
                Items = new List<CartItem>()
            };
        }
    }
}