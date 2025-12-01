using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.Common;

/// <summary>
/// API response model for CreateProduct operation
/// </summary>
public class ProductResponse
{
    /// <summary>
    /// The unique identifier of the created product
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The product title
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// The product description
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// The product category
    /// </summary>
    public ProductCategory Category { get; set; }

    /// <summary>
    /// The product price
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// The URL of the product image
    /// </summary>
    public string Image { get; set; } = string.Empty;

    /// <summary>
    /// The product rating
    /// </summary>
    public ProductRatingResponse Rating { get; set; } = new ProductRatingResponse();
}
