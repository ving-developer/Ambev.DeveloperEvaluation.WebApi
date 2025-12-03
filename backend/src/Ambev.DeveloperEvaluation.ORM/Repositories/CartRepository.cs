using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Implementation of ICartRepository using Entity Framework Core
/// </summary>
public class CartRepository : RepositoryBase<Cart>, ICartRepository
{
    public CartRepository(DefaultContext context) : base(context) { }

    public Task<Cart?> GetBySaleNumberAsync(string saleNumber, CancellationToken cancellationToken = default)
    {
        return GetAsync(p => p.SaleNumber == saleNumber, cancellationToken);
    }
}