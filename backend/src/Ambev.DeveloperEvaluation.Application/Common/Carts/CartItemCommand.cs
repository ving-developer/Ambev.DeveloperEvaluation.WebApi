namespace Ambev.DeveloperEvaluation.Application.Common.Carts;

/// <summary>
/// Represents a cart item used in cart-related commands.
/// </summary>
public record CartItemCommand
{
    /// <summary>
    /// Unique identifier of the cart item.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Identifier of the product.
    /// </summary>
    public Guid ProductId { get; init; }

    /// <summary>
    /// Quantity of the product in the cart.
    /// </summary>
    public int Quantity { get; init; }
}
