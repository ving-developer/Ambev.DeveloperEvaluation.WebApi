using Ambev.DeveloperEvaluation.WebApi.Features.Carts.UpdateItemQuantity;

namespace Ambev.DeveloperEvaluation.Integration.Shared.TestData.Carts;

/// <summary>
/// Provides methods for generating test data for UpdateItemQuantityRequest.
/// </summary>
internal static class UpdateItemQuantityRequestTestData
{
    /// <summary>
    /// Generates a valid UpdateItemQuantityRequest with a specified quantity.
    /// </summary>
    /// <param name="quantity">The quantity to set for the item</param>
    public static UpdateItemQuantityRequest GetValidUpdateItemQuantityRequest(int quantity = 1)
    {
        return new UpdateItemQuantityRequest
        {
            Quantity = quantity
        };
    }
}