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
    public Guid ProductId { get; private set; }

    /// <summary>
    /// Product associated with this rating.
    /// </summary>
    public Product Product { get; private set; } = default!;

    /// <summary>
    /// Gets the average rating of this evaluation.
    /// </summary>
    public decimal Rate { get; private set; }

    /// <summary>
    /// Gets the number of ratings has received.
    /// </summary>
    public int Count { get; private set; }

    /// <summary>
    /// Public constructor, used to instantiate Rating entity
    /// </summary>
    /// <param name="rate"></param>
    /// <param name="count"></param>
    public Rating(decimal rate, int count)
    {
        Rate = rate;
        Count = count;
    }
}
