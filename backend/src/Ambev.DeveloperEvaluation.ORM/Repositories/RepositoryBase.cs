using Ambev.DeveloperEvaluation.Common.Pagination;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Base repository that implements common CRUD operations for any entity
/// that inherits from BaseEntity.
/// </summary>
/// <typeparam name="TEntity">The type of the entity</typeparam>
public abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : BaseEntity
{
    protected readonly DbContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    protected RepositoryBase(DbContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    public virtual async Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public virtual async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public virtual async Task<bool> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public virtual async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public virtual async Task<TEntity?> GetByIdAsync(
    Guid id,
    CancellationToken cancellationToken = default,
    params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _dbSet;

        if (includes != null && includes.Length > 0)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        return await query.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }


    public virtual async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity == null) return false;
        return await DeleteAsync(entity, cancellationToken);
    }

    public virtual async Task<List<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = _dbSet;
        if (predicate != null)
            query = query.Where(predicate);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<PaginatedResponse<TEntity>> GetPaginatedAsync(
        Expression<Func<TEntity, bool>>? predicate,
        int pageNumber,
        int pageSize,
        string? orderBy = null,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = _dbSet;

        if (predicate != null)
            query = query.Where(predicate);

        if (!string.IsNullOrWhiteSpace(orderBy))
        {
            var param = Expression.Parameter(typeof(TEntity), "x");
            var property = Expression.PropertyOrField(param, orderBy);
            var lambda = Expression.Lambda(property, param);

            var method = typeof(Queryable).GetMethods()
                .First(m => m.Name == "OrderBy" && m.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(TEntity), property.Type);

            query = (IQueryable<TEntity>)method.Invoke(null, [query, lambda])!;
        }
        else
        {
            query = query.OrderBy(x => EF.Property<object>(x, "Id"));
        }

        var count = await query.CountAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling(count / (double)pageSize);
        var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

        return new PaginatedResponse<TEntity>(items, pageNumber, totalPages, count);
    }
}
