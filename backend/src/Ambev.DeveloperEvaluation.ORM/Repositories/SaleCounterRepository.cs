using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Implementation of ISaleCounterRepository using Entity Framework Core
/// </summary>
public class SaleCounterRepository : RepositoryBase<SaleCounter>, ISaleCounterRepository
{
    public SaleCounterRepository(DefaultContext context) : base(context) { }

    public async Task<long> GetAndIncrementSaleNumberAsync(Guid branchId, CancellationToken cancellationToken = default)
    {
        var saleCounter = await GetAsync(sc => sc.BranchId == branchId, cancellationToken);

        if (saleCounter == null)
        {
            saleCounter = new SaleCounter(branchId);
            await CreateAsync(saleCounter, cancellationToken);
            return saleCounter.LastNumber;
        }

        var nextNumber = saleCounter.GetNextSaleNumber();

        await UpdateAsync(saleCounter, cancellationToken);
        return nextNumber;
    }
}