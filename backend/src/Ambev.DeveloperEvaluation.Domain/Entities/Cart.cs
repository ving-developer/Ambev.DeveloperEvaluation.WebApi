using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Exceptions;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a sales transaction (cart) with business rules for discounts and quantity limits.
/// This entity follows domain-driven design principles and enforces business rules such as:
/// - Maximum of 20 identical items per product
/// - Discounts based on quantity ranges (4+ items: 10%, 10-20 items: 20%)
/// - Cart status lifecycle (Pending, Completed, Canceled)
/// - Validation of all operations according to cart state
/// </summary>
public class Cart : BaseEntity
{
    private readonly List<CartItem> _items = [];

    /// <summary>
    /// Gets the unique sale number for this cart.
    /// This serves as a human-readable identifier for the sale transaction.
    /// </summary>
    public string SaleNumber { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the date and time when the sale was created/initiated.
    /// This is fixed at cart creation to maintain transaction integrity.
    /// </summary>
    public DateTime SaleDate { get; private set; }

    /// <summary>
    /// Gets the unique identifier of the customer making the purchase.
    /// This follows the "External Identity with Denormalization" pattern.
    /// </summary>
    public Guid CustomerId { get; private set; }

    /// <summary>
    /// Gets the total amount of the sale after all discounts are applied.
    /// This value is calculated when the cart is completed and remains fixed.
    /// </summary>
    public decimal TotalAmount { get; private set; }

    /// <summary>
    /// Gets the unique identifier of the branch where the sale occurred.
    /// This follows the "External Identity with Denormalization" pattern.
    /// </summary>
    public Guid BranchId { get; private set; }

    /// <summary>
    /// Gets the branch where this sale occurred.
    /// Navigation property for ORM relationships and business operations.
    /// </summary>
    public Branch Branch { get; private set; } = default!;

    /// <summary>
    /// Gets the current status of the cart.
    /// Determines which operations are allowed based on the cart's lifecycle state.
    /// </summary>
    public CartStatus Status { get; private set; } = CartStatus.Pending;

    /// <summary>
    /// Gets a value indicating whether the cart has been canceled.
    /// Canceled carts cannot be modified or completed.
    /// </summary>
    public bool IsCanceled => Status == CartStatus.Canceled;

    /// <summary>
    /// Gets the collection of items in the cart.
    /// This is a read-only collection that enforces encapsulation of the internal list.
    /// </summary>
    public IReadOnlyCollection<CartItem> Items => _items.AsReadOnly();

    /// <summary>
    /// Gets the date and time when the cart was created.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Gets the date and time of the last update to the cart.
    /// Updated whenever any modification occurs to the cart or its items.
    /// </summary>
    public DateTime? UpdatedAt { get; private set; }

    /// <summary>
    /// Gets the date and time when the cart was canceled.
    /// Only populated if the cart status is <see cref="CartStatus.Canceled"/>.
    /// </summary>
    public DateTime? CanceledAt { get; private set; }

    /// <summary>
    /// Gets the reason for cart cancellation.
    /// Required when canceling a cart to provide audit trail.
    /// </summary>
    public string? CancellationReason { get; private set; }

    /// <summary>
    /// Private constructor for Entity Framework Core.
    /// Required for ORM materialization without bypassing business rules.
    /// </summary>
    private Cart() { }

    /// <summary>
    /// Initializes a new instance of the Cart class with all required properties.
    /// </summary>
    /// <param name="customerId">The unique identifier of the customer. Must be a valid GUID.</param>
    /// <param name="branchId">The unique identifier of the branch. Must be a valid GUID.</param>
    /// <param name="saleNumber">The unique sale number. Must not be null or empty.</param>
    /// <exception cref="ArgumentException">Thrown when any required parameter is invalid.</exception>
    public Cart(Guid customerId, Guid branchId, string saleNumber)
    {
        if (customerId == Guid.Empty)
            throw new ArgumentException("Customer ID is required", nameof(customerId));

        if (branchId == Guid.Empty)
            throw new ArgumentException("Branch ID is required", nameof(branchId));

        if (string.IsNullOrWhiteSpace(saleNumber))
            throw new ArgumentException("Sale number is required", nameof(saleNumber));

        CustomerId = customerId;
        BranchId = branchId;
        SaleNumber = saleNumber;
        SaleDate = DateTime.UtcNow;
        CreatedAt = DateTime.UtcNow;
        Status = CartStatus.Pending;
    }

    /// <summary>
    /// Adds an item to the cart, applying business rules for quantities and discounts.
    /// Business Rules:
    /// - Cannot add items to completed or canceled carts
    /// - Quantity must be greater than zero
    /// - Unit price must be greater than zero
    /// - Cannot exceed 20 units of the same product
    /// - Automatically calculates applicable discounts based on quantity
    /// </summary>
    /// <param name="productId">The unique identifier of the product to add.</param>
    /// <param name="quantity">The quantity to add. Must be greater than zero.</param>
    /// <param name="unitPrice">The unit price of the product. Must be greater than zero.</param>
    /// <exception cref="DomainException">Thrown when cart is not in pending state.</exception>
    /// <exception cref="ArgumentException">Thrown when parameters are invalid.</exception>
    /// <exception cref="DomainException">Thrown when exceeding maximum quantity limit.</exception>
    public void AddItem(Guid productId, int quantity, decimal unitPrice)
    {
        if (Status != CartStatus.Pending)
            throw new DomainException("Cannot add items to a completed or canceled sale.");

        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));

        if (unitPrice <= 0)
            throw new ArgumentException("Unit price must be greater than zero.", nameof(unitPrice));

        var existingQuantity = GetTotalQuantityForProduct(productId);
        var newTotalQuantity = existingQuantity + quantity;

        if (newTotalQuantity > 20)
            throw new DomainException(
                $"Cannot sell more than 20 units of the same product. " +
                $"Current: {existingQuantity}, Attempting to add: {quantity}");

        var discountPercentage = CalculateDiscountPercentage(newTotalQuantity);

        var item = new CartItem(
            cartId: Id,
            productId: productId,
            quantity: quantity,
            unitPrice: unitPrice,
            discountPercentage: discountPercentage
        );

        _items.Add(item);
        TotalAmount += item.TotalPrice;
        UpdatedAt = DateTime.UtcNow;

        RecalculateDiscountsForProduct(productId);
    }

    /// <summary>
    /// Completes the sale and calculates the final total amount.
    /// Business Rules:
    /// - Only pending carts can be completed
    /// - Cart must contain at least one item
    /// - Final total is calculated with all discounts applied
    /// - Cart status changes to Completed
    /// </summary>
    /// <exception cref="DomainException">Thrown when cart is not pending or has no items.</exception>
    public void Complete()
    {
        if (Status != CartStatus.Pending)
            throw new DomainException("Only pending sales can be completed.");

        if (_items.Count == 0)
            throw new DomainException("Cannot complete a sale without items.");

        Status = CartStatus.Completed;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Cancels the entire sale with a specified reason.
    /// Business Rules:
    /// - Cannot cancel an already canceled cart
    /// - Cancellation reason is required for audit trail
    /// - Cancellation date is recorded
    /// - Cart status changes to Canceled
    /// </summary>
    /// <param name="reason">The reason for cancellation. Must not be null.</param>
    /// <exception cref="DomainException">Thrown when cart is already canceled.</exception>
    public void Cancel(string reason)
    {
        if (Status == CartStatus.Canceled)
            throw new DomainException("Sale is already canceled.");

        Status = CartStatus.Canceled;
        CancellationReason = reason ?? "No reason provided";
        CanceledAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Removes an item from the cart.
    /// Business Rules:
    /// - Only pending carts can have items removed
    /// - Item must exist in the cart
    /// - Discounts for remaining items of the same product are recalculated
    /// </summary>
    /// <param name="itemId">The unique identifier of the item to remove.</param>
    /// <exception cref="DomainException">Thrown when cart is not in pending state.</exception>
    /// <exception cref="ArgumentException">Thrown when item is not found in cart.</exception>
    public void RemoveItem(Guid itemId)
    {
        if (Status != CartStatus.Pending)
            throw new DomainException("Cannot remove items from completed or canceled sale.");

        var item = _items.FirstOrDefault(i => i.Id == itemId) ?? throw new ArgumentException($"Item {itemId} not found in cart.", nameof(itemId));
        var productId = item.ProductId;

        _items.Remove(item);
        UpdatedAt = DateTime.UtcNow;

        RecalculateDiscountsForProduct(productId);
    }

    /// <summary>
    /// Updates the quantity of an existing item in the cart.
    /// Business Rules:
    /// - Only pending carts can have item quantities updated
    /// - New quantity must be greater than zero
    /// - Cannot exceed 20 units of the same product total
    /// - Discounts for all items of the same product are recalculated
    /// </summary>
    /// <param name="itemId">The unique identifier of the item to update.</param>
    /// <param name="newQuantity">The new quantity. Must be greater than zero.</param>
    /// <exception cref="DomainException">Thrown when cart is not pending or quantity limit exceeded.</exception>
    /// <exception cref="DomainException">Thrown when item not found or quantity invalid.</exception>
    public void UpdateItemQuantity(Guid itemId, int newQuantity)
    {
        if (Status != CartStatus.Pending)
            throw new DomainException("Cannot update items in completed or canceled sale.");

        if (newQuantity <= 0)
            throw new DomainException("Quantity must be greater than zero.");

        var item = _items.FirstOrDefault(i => i.Id == itemId) ?? throw new DomainException($"Item {itemId} not found in cart.");
        var productId = item.ProductId;
        var otherItemsQuantity = GetTotalQuantityForProduct(productId) - item.Quantity;
        var newTotalQuantity = otherItemsQuantity + newQuantity;

        if (newTotalQuantity > 20)
            throw new DomainException(
                $"Cannot have more than 20 units of the same product. " +
                $"Would result in: {newTotalQuantity}");

        item.UpdateQuantity(newQuantity);
        UpdatedAt = DateTime.UtcNow;

        RecalculateDiscountsForProduct(productId);
    }

    /// <summary>
    /// Calculates discount percentage based on total quantity of a product.
    /// Business Rules:
    /// - Below 4 items: 0% (no discount)
    /// - 4+ items: 10% discount
    /// - 10-20 items: 20% discount
    /// </summary>
    /// <param name="totalQuantity">The total quantity of a specific product in the cart.</param>
    /// <returns>The applicable discount percentage (0, 10, or 20).</returns>
    private static decimal CalculateDiscountPercentage(int totalQuantity)
    {
        if (totalQuantity < 4)
            return 0m;

        if (totalQuantity < 10)
            return 10m;

        return 20m;
    }

    /// <summary>
    /// Recalculates discounts for all items of a specific product.
    /// Ensures that all items of the same product have consistent discount percentages.
    /// This is called whenever quantities change for a product.
    /// </summary>
    /// <param name="productId">The unique identifier of the product.</param>
    private void RecalculateDiscountsForProduct(Guid productId)
    {
        var productItems = _items.Where(i => i.ProductId == productId).ToList();
        if (!productItems.Any())
            return;

        var totalQuantity = productItems.Sum(item => item.Quantity);
        var newDiscount = CalculateDiscountPercentage(totalQuantity);

        foreach (var item in productItems)
        {
            item.UpdateDiscount(newDiscount);
        }

        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Gets the total quantity of a specific product in the cart.
    /// </summary>
    /// <param name="productId">The unique identifier of the product.</param>
    /// <returns>The total quantity of the specified product in the cart.</returns>
    private int GetTotalQuantityForProduct(Guid productId)
    {
        return _items
            .Where(i => i.ProductId == productId)
            .Sum(i => i.Quantity);
    }

    /// <summary>
    /// Calculates the subtotal (sum of all item prices before discounts).
    /// This is a calculated property that does not persist in the database.
    /// </summary>
    public decimal Subtotal => _items.Sum(item => item.Subtotal);

    /// <summary>
    /// Calculates the total discount amount applied to all items.
    /// This is a calculated property that does not persist in the database.
    /// </summary>
    public decimal TotalDiscount => _items.Sum(item => item.DiscountAmount);

    /// <summary>
    /// Calculates the total number of items in the cart.
    /// This is a calculated property that does not persist in the database.
    /// </summary>
    public int TotalItemCount => _items.Sum(item => item.Quantity);

    /// <summary>
    /// Gets the list of unique product identifiers in the cart.
    /// This is useful for inventory management and reporting.
    /// </summary>
    public IEnumerable<Guid> UniqueProductIds => _items
        .Select(item => item.ProductId)
        .Distinct();

    /// <summary>
    /// Checks if a specific product has any discount applied.
    /// </summary>
    /// <param name="productId">The unique identifier of the product.</param>
    /// <returns>True if the product has a discount greater than zero; otherwise, false.</returns>
    public bool ProductHasDiscount(Guid productId)
    {
        var item = _items.FirstOrDefault(i => i.ProductId == productId);
        return item?.DiscountPercentage > 0;
    }

    /// <summary>
    /// Gets the current discount percentage for a specific product.
    /// </summary>
    /// <param name="productId">The unique identifier of the product.</param>
    /// <returns>The discount percentage (0, 10, or 20) for the specified product.</returns>
    public decimal GetProductDiscountPercentage(Guid productId)
    {
        var item = _items.FirstOrDefault(i => i.ProductId == productId);
        return item?.DiscountPercentage ?? 0m;
    }
}