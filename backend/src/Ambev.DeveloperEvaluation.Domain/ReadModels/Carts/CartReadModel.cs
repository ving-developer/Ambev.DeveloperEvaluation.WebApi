namespace Ambev.DeveloperEvaluation.Domain.ReadModels.Carts;

/// <summary>
/// Represents a read-only cart model used for query operations.
/// </summary>
public record CartReadModel
{
    /// <summary>Cart identifier.</summary>
    public Guid Id { get; init; }

    /// <summary>Associated sale number.</summary>
    public string SaleNumber { get; init; } = string.Empty;

    /// <summary>Date of the sale.</summary>
    public DateTime SaleDate { get; init; }

    /// <summary>Customer identifier.</summary>
    public Guid CustomerId { get; init; }

    /// <summary>Total cart amount.</summary>
    public decimal TotalAmount { get; init; }

    /// <summary>Branch identifier.</summary>
    public Guid BranchId { get; init; }

    /// <summary>Branch name.</summary>
    public string BranchName { get; init; } = string.Empty;

    /// <summary>Current cart status.</summary>
    public string Status { get; init; } = string.Empty;

    /// <summary>Indicates whether the cart is canceled.</summary>
    public bool IsCanceled { get; init; }

    /// <summary>Creation date.</summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>Last update date.</summary>
    public DateTime? UpdatedAt { get; init; }

    /// <summary>Cancellation date, if applicable.</summary>
    public DateTime? CanceledAt { get; init; }

    /// <summary>Cancellation reason, if any.</summary>
    public string? CancellationReason { get; init; }
}
