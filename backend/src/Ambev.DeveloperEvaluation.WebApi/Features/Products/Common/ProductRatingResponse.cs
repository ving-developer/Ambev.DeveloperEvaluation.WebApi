namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.Common;

/// <summary>
/// Response model for product rating
/// </summary>
public class ProductRatingResponse
{
    /// <summary>
    /// The average rate of the product
    /// </summary>
    public decimal Rate { get; set; }

    /// <summary>
    /// The number of ratings the product has received
    /// </summary>
    public int Count { get; set; }
}
