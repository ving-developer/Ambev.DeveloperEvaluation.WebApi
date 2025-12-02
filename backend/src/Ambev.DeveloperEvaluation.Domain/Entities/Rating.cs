using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents the evaluation rating.
/// </summary>
public class Rating : BaseEntity
{
    /// <summary>
    /// Relation of Rating and Products
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets the average rating of this evaluation.
    /// </summary>
    public decimal Rate { get; set; }

    /// <summary>
    /// Gets the number of ratings has received.
    /// </summary>
    public int Count { get; set; }
}
