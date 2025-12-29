using Ambev.DeveloperEvaluation.Common.Pagination;
using Ambev.DeveloperEvaluation.Domain.ReadModels.Carts;

namespace Ambev.DeveloperEvaluation.Domain.Queries.Carts;

public interface ICartQuery
{
    Task<CartReadModel?> GetByIdAsync(Guid cartId, CancellationToken ct);
    Task<IReadOnlyList<CartProductReadModel>> GetItemsAsync(Guid cartId, CancellationToken ct);
    Task<PaginatedResponse<CartReadModel>> SearchAsync(CartSearchFilter filter, CancellationToken ct);
}
