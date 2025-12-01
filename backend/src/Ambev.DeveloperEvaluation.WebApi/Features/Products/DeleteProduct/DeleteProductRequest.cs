namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.DeleteProduct;

/// <summary>
/// Request model for deleting a Product
/// </summary>
public class DeleteProductRequest
{
    /// <summary>
    /// The unique identifier of the product to delete
    /// </summary>
    public Guid Id { get; set; }
}
