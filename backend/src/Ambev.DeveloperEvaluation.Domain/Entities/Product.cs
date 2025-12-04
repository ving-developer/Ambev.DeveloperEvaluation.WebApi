using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a product in the system.
/// This entity follows domain-driven design principles and includes business rules validation.
/// </summary>
public class Product : BaseEntity
{
    /// <summary>
    /// Gets the product title.
    /// Must not be null or empty.
    /// </summary>
    public string Title { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the product price.
    /// Must be a positive value.
    /// </summary>
    public decimal Price { get; private set; }

    /// <summary>
    /// Gets the product description.
    /// </summary>
    public string Description { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the product category.
    /// Must not be null or empty.
    /// </summary>
    public ProductCategory Category { get; private set; }

    /// <summary>
    /// Gets the URL of the product image.
    /// </summary>
    public string Image { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the product rating.
    /// </summary>
    public Rating? Rating { get; private set; }

    /// <summary>
    /// Gets the date and time when the product was created.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Gets the date and time of the last update to the product.
    /// </summary>
    public DateTime? UpdatedAt { get; private set; }

    /// <summary>
    /// Initializes a new instance of the Product class.
    /// </summary>
    public Product()
    {
        CreatedAt = DateTime.UtcNow;
    }
}
