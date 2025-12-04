using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a counter for sale numbers per branch.
/// Each branch has its own independent counter that starts at 0.
/// </summary>
public class SaleCounter : BaseEntity
{
    /// <summary>
    /// Gets the branch identifier.
    /// Foreign key to the Branch entity.
    /// </summary>
    public Guid BranchId { get; private set; }

    /// <summary>
    /// Gets the branch associated with this counter.
    /// Navigation property for ORM relationships.
    /// </summary>
    public Branch Branch { get; private set; } = default!;

    /// <summary>
    /// Gets the last sale number generated for this branch.
    /// This value is incremented each time a new sale is created.
    /// </summary>
    public long LastNumber { get; private set; }

    /// <summary>
    /// Private constructor for Entity Framework Core.
    /// Required for ORM materialization.
    /// </summary>
    private SaleCounter() { }

    /// <summary>
    /// Initializes a new instance of the SaleCounter class.
    /// </summary>
    /// <param name="branchId">The branch identifier. Must be a valid GUID.</param>
    /// <exception cref="ArgumentException">Thrown when branchId is empty.</exception>
    public SaleCounter(Guid branchId)
    {
        if (branchId == Guid.Empty)
            throw new ArgumentException("Branch ID cannot be empty.", nameof(branchId));

        BranchId = branchId;
        LastNumber = 1;
    }

    /// <summary>
    /// Gets the next sale number for this branch.
    /// Increments the counter and returns the formatted number.
    /// </summary>
    /// <returns>The next sale number formatted as 6-digit string.</returns>
    public long GetNextSaleNumber()
    {
        return ++LastNumber;
    }
}