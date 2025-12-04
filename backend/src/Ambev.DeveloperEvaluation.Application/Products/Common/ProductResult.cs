using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Application.Products.Common;

/// <summary>
/// Represents the response returned after successfully creating a new product.
/// </summary>
/// <remarks>
/// This response contains the unique identifier of the newly created product,
/// along with its details and initial rating, which can be used for subsequent
/// operations or reference.
/// </remarks>
public record ProductResult
{
    /// <summary>
    /// Gets or sets the unique identifier of the newly created product.
    /// </summary>
    /// <value>A GUID that uniquely identifies the created product in the system.</value>
    public Guid Id { get; set; }

    /// <summary>
    /// The product title.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// The product description.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// The product category.
    /// </summary>
    public ProductCategory Category { get; set; }

    /// <summary>
    /// The product price.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// The URL of the product image.
    /// </summary>
    public string Image { get; set; } = string.Empty;

    /// <summary>
    /// The product rating.
    /// </summary>
    public ProductRating Rating { get; set; } = new ProductRating();
}
