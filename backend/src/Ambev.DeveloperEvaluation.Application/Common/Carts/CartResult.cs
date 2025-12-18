namespace Ambev.DeveloperEvaluation.Application.Common.Carts;

/// <summary>
/// Represents a cart summary with its related items.
/// </summary>
public record CartResult
{
    /// <summary>
    /// Unique identifier of the cart.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Sale number associated with the cart.
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;

    /// <summary>
    /// Date when the sale was created.
    /// </summary>
    public DateTime SaleDate { get; set; }

    /// <summary>
    /// Identifier of the customer.
    /// </summary>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// Total amount of the cart.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Identifier of the branch.
    /// </summary>
    public Guid BranchId { get; set; }

    /// <summary>
    /// Name of the branch.
    /// </summary>
    public string BranchName { get; set; } = string.Empty;

    /// <summary>
    /// Current status of the cart.
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Indicates whether the cart was canceled.
    /// </summary>
    public bool IsCanceled { get; set; }

    /// <summary>
    /// Date when the cart was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Date when the cart was last updated.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Date when the cart was canceled.
    /// </summary>
    public DateTime? CanceledAt { get; set; }

    /// <summary>
    /// Reason for cart cancellation, if applicable.
    /// </summary>
    public string? CancellationReason { get; set; }

    /// <summary>
    /// Items contained in the cart.
    /// </summary>
    public List<CartItemCommand> Items { get; set; } = new();
}
