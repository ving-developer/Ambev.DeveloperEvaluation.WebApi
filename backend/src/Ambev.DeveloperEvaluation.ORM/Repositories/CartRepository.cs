using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class CartRepository : RepositoryBase<Cart>, ICartRepository
{
    public CartRepository(DefaultContext context) : base(context) { }

    public override async Task<Cart?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default,
        params Expression<Func<Cart, object>>[] includes)
    {
        return await _dbSet
            .Include(c => c.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public Task<Cart?> GetBySaleNumberAsync(string saleNumber, CancellationToken cancellationToken = default)
    {
        return GetAsync(p => p.SaleNumber == saleNumber, cancellationToken);
    }
}
