using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Repository interface for Product entity operations
/// </summary>
public interface IProductRepository : IRepositoryBase<Product>
{
    /// <summary>
    /// Retrieves a product by its title
    /// </summary>
    /// <param name="title">The title of the product</param>
    /// <param name="cancellationToken">Optional cancellation token</param>
    /// <returns>The product if found, null otherwise</returns>
    Task<Product?> GetByTitleAsync(string title, CancellationToken cancellationToken = default);
}
