using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

public interface ISaleCounterRepository : IRepositoryBase<SaleCounter>
{
    /// <summary>
    /// Increment the next sale number for a branch and increments the counter.
    /// This method handles the transaction and locking internally.
    /// </summary>
    /// <param name="branchId">The branch identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The next sale number.</returns>
    Task<long> GetAndIncrementSaleNumberAsync(Guid branchId, CancellationToken cancellationToken = default);
}