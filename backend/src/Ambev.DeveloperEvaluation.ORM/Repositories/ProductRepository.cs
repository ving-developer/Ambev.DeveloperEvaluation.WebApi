using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Implementation of IProductRepository using Entity Framework Core
/// </summary>
public class ProductRepository : RepositoryBase<Product>, IProductRepository
{
    public ProductRepository(DefaultContext context) : base(context) { }

    public Task<Product?> GetByTitleAsync(string title, CancellationToken cancellationToken = default)
    {
        return GetAsync(p => p.Title == title, cancellationToken);
    }
}