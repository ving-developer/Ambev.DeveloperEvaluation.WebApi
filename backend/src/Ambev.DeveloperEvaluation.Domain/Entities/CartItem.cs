using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents an item in a shopping cart.
/// This entity follows domain-driven design principles and includes business rules validation for cart items.
/// </summary>
public class CartItem : BaseEntity
{
    /// <summary>
    /// Gets the unique identifier of the cart this item belongs to.
    /// This is a foreign key to the Cart entity.
    /// </summary>
    public Guid CartId { get; private set; }

    /// <summary>
    /// Gets the cart that contains this item.
    /// Navigation property for ORM relationships.
    /// </summary>
    public Cart Cart { get; private set; } = default!;

    /// <summary>
    /// Gets the unique identifier of the product in this cart item.
    /// This is a foreign key to the Product entity.
    /// </summary>
    public Guid ProductId { get; private set; }

    /// <summary>
    /// Gets the product associated with this cart item.
    /// Navigation property for ORM relationships.
    /// </summary>
    public Product Product { get; private set; } = default!;

    /// <summary>
    /// Gets the quantity of the product in this cart item.
    /// Must be greater than zero and respect business rules for maximum quantities.
    /// </summary>
    public int Quantity { get; private set; }

    /// <summary>
    /// Gets the unit price of the product at the time it was added to the cart.
    /// This price is fixed when the item is added to ensure consistency.
    /// </summary>
    public decimal UnitPrice { get; private set; }

    /// <summary>
    /// Gets the discount percentage applied to this item.
    /// Must be between 0 and 100 percent.
    /// </summary>
    public decimal DiscountPercentage { get; private set; }

    /// <summary>
    /// Gets the date and time when the cart item was created.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Private constructor for Entity Framework Core.
    /// Required for ORM materialization.
    /// </summary>
    private CartItem() { }

    /// <summary>
    /// Initializes a new instance of the CartItem class with all required properties.
    /// </summary>
    /// <param name="cartId">The unique identifier of the cart. Must be a valid GUID.</param>
    /// <param name="productId">The unique identifier of the product. Must be a valid GUID.</param>
    /// <param name="quantity">The quantity of the product. Must be greater than zero.</param>
    /// <param name="unitPrice">The unit price of the product. Must be greater than or equal to zero.</param>
    /// <param name="discountPercentage">The discount percentage to apply. Must be between 0 and 100.</param>
    /// <exception cref="ArgumentException">Thrown when any parameter fails validation.</exception>
    public CartItem(Guid cartId, Guid productId, int quantity, decimal unitPrice, decimal discountPercentage)
    {
        if (cartId == Guid.Empty)
            throw new ArgumentException("Cart ID cannot be empty.", nameof(cartId));

        if (productId == Guid.Empty)
            throw new ArgumentException("Product ID cannot be empty.", nameof(productId));

        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));

        if (unitPrice < 0)
            throw new ArgumentException("Unit price cannot be negative.", nameof(unitPrice));

        if (discountPercentage < 0 || discountPercentage > 100)
            throw new ArgumentException("Discount percentage must be between 0 and 100.", nameof(discountPercentage));

        CartId = cartId;
        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
        DiscountPercentage = discountPercentage;
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Calculates the total value without discount (quantity × unit price).
    /// This is a calculated property that does not persist in the database.
    /// </summary>
    public decimal Subtotal => Quantity * UnitPrice;

    /// <summary>
    /// Calculates the discount amount in reais.
    /// This is a calculated property that does not persist in the database.
    /// </summary>
    public decimal DiscountAmount => Subtotal * (DiscountPercentage / 100);

    /// <summary>
    /// Calculates the final price with discount applied.
    /// This is a calculated property that does not persist in the database.
    /// </summary>
    public decimal TotalPrice => Subtotal - DiscountAmount;

    /// <summary>
    /// Updates the discount percentage for this cart item.
    /// </summary>
    /// <param name="newDiscountPercentage">The new discount percentage to apply. Must be between 0 and 100.</param>
    /// <exception cref="ArgumentException">Thrown when the discount percentage is not within the valid range.</exception>
    internal void UpdateDiscount(decimal newDiscountPercentage)
    {
        if (newDiscountPercentage < 0 || newDiscountPercentage > 100)
            throw new ArgumentException("Discount percentage must be between 0 and 100.", nameof(newDiscountPercentage));

        DiscountPercentage = newDiscountPercentage;
    }

    /// <summary>
    /// Updates the quantity of this cart item.
    /// </summary>
    /// <param name="newQuantity">The new quantity. Must be greater than zero.</param>
    /// <exception cref="ArgumentException">Thrown when the quantity is not greater than zero.</exception>
    internal void UpdateQuantity(int newQuantity)
    {
        if (newQuantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(newQuantity));

        Quantity = newQuantity;
    }
}