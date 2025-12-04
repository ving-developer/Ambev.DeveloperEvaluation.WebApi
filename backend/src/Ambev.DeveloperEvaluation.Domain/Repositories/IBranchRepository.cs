using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Repository interface for Branch entity - Minimum required for sales operations
/// </summary>
public interface IBranchRepository : IRepositoryBase<Branch>
{
    /// <summary>
    /// Retrieves a branch by code
    /// </summary>
    /// <param name="code">The code number to search for</param>
    /// <param name="cancellationToken">Optional cancellation token</param>
    /// <returns>The branch if found, null otherwise</returns>
    Task<Branch?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
}