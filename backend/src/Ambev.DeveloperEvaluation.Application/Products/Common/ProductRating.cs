namespace Ambev.DeveloperEvaluation.Application.Products.Common;

/// <summary>
/// Represent's product rating.
/// </summary>
public record ProductRating
{
    /// <summary>
    /// The average rate of the product.
    /// </summary>
    public decimal Rate { get; set; } = 0;

    /// <summary>
    /// The number of ratings the product has received.
    /// </summary>
    public int Count { get; set; } = 0;
}
