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

    [System.Text.Json.Serialization.JsonConstructor]
    public ListCartsCommand(
        int page = 1,
        int pageSize = 10,
        string? orderBy = null,
        Guid? customerId = null,
        Guid? branchId = null,
        CartStatus? status = null,
        DateTime? startDate = null,
        DateTime? endDate = null)
        : base(page, pageSize, orderBy) // Chama o construtor da classe base
    {
        CustomerId = customerId;
        BranchId = branchId;
        Status = status;
        StartDate = startDate;
        EndDate = endDate;
    }
}