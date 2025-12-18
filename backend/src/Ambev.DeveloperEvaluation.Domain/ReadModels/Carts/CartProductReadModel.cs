namespace Ambev.DeveloperEvaluation.Domain.ReadModels.Carts;

/// <summary>
/// Represents a read-only cart product model used for query operations.
/// </summary>
public record CartProductReadModel
{
    /// <summary>
    /// Identifier of this cart item.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Identifier of the product.
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Quantity of the product in the cart.
    /// </summary>
    public int Quantity { get; set; }
}
