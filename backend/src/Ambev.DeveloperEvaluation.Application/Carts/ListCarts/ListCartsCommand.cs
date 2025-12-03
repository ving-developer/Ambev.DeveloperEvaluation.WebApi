using Ambev.DeveloperEvaluation.Application.Carts.Common;
using Ambev.DeveloperEvaluation.Common.Pagination;
using Ambev.DeveloperEvaluation.Domain.Enums;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.ListCarts;

public record ListCartsCommand : PaginationParams, IRequest<PaginatedResponse<CartResult>>
{
    public Guid? CustomerId { get; init; }
    public Guid? BranchId { get; init; }
    public CartStatus? Status { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
}