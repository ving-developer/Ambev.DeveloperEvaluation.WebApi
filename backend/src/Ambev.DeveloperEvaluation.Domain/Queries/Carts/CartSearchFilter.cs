using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Domain.Queries.Carts;

/// <summary>
/// Filter used to search carts with pagination and ordering.
/// </summary>
public sealed class CartSearchFilter
{
    /// <summary>
    /// Page number (1-based).
    /// </summary>
    public int Page { get; init; } = 1;

    /// <summary>
    /// Page size.
    /// </summary>
    public int PageSize { get; init; } = 10;

    /// <summary>
    /// Order expression (e.g. "createdAt desc").
    /// </summary>
    public string? Order { get; init; }

    /// <summary>
    /// Customer identifier filter.
    /// </summary>
    public Guid? CustomerId { get; init; }

    /// <summary>
    /// Branch identifier filter.
    /// </summary>
    public Guid? BranchId { get; init; }

    /// <summary>
    /// Cart status filter.
    /// </summary>
    public CartStatus? Status { get; init; }
}
