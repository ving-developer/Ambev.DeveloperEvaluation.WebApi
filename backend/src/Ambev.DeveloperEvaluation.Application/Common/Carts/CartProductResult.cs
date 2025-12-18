namespace Ambev.DeveloperEvaluation.Application.Common.Carts;

/// <summary>
/// Represents a product and its quantity in a cart.
/// </summary>
public record CartProductResult
{
    /// <summary>
    /// Identifier of the product.
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Quantity of the product in the cart.
    /// </summary>
    public int Quantity { get; set; }
}
