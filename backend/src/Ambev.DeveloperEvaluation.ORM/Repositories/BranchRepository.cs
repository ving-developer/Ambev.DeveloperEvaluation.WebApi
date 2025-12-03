using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Implementation of IBranchRepository using Entity Framework Core
/// </summary>
public class BranchRepository : RepositoryBase<Branch>, IBranchRepository
{
    public BranchRepository(DefaultContext context) : base(context) { }

    /// <summary>
    /// Implementation to retrieves a branch by code
    /// </summary>
    /// <param name="code">The code number to search for</param>
    /// <param name="cancellationToken">Optional cancellation token</param>
    /// <returns>The branch if found, null otherwise</returns>
    public Task<Branch?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return GetAsync(u => u.Code == code, cancellationToken);
    }
}