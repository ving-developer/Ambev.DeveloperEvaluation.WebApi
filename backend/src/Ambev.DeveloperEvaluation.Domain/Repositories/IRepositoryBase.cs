using Ambev.DeveloperEvaluation.Domain.Common;
using System.Linq.Expressions;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Generic repository interface that defines common CRUD operations
/// for any entity that inherits from <see cref="BaseEntity"/>.
/// </summary>
/// <typeparam name="TEntity">The type of entity.</typeparam>
public interface IRepositoryBase<TEntity> where TEntity : BaseEntity
{
    /// <summary>
    /// Creates a new entity in the repository.
    /// </summary>
    /// <param name="entity">The entity to create.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>The created entity.</returns>
    Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing entity in the repository.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>The updated entity.</returns>
    Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an entity from the repository.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>True if deletion was successful, otherwise false.</returns>
    Task<bool> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to delete.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>True if deletion was successful, otherwise false.</returns>
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a single entity matching the specified predicate.
    /// </summary>
    /// <param name="predicate">The filter expression to locate the entity.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>The entity if found, otherwise null.</returns>
    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves an entity by its unique identifier, with optional related entities included.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <param name="includes">Navigation properties to include.</param>
    /// <returns>The entity if found, otherwise null.</returns>
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includes);

    /// <summary>
    /// Retrieves all entities optionally filtered by a predicate.
    /// </summary>
    /// <param name="predicate">Optional filter expression.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>A list of entities matching the filter.</returns>
    Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default);
}
