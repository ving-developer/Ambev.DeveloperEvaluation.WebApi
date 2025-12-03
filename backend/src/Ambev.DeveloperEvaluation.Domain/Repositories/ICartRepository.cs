using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Repository interface for Cart entity operations
/// </summary>
public interface ICartRepository : IRepositoryBase<Cart>
{
    /// <summary>
    /// Gets a cart by its sale number (for validation)
    /// </summary>
    Task<Cart?> GetBySaleNumberAsync(string saleNumber, CancellationToken cancellationToken = default);
}