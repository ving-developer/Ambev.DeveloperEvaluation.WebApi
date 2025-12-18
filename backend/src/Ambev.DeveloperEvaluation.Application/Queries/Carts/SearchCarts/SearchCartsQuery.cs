using Ambev.DeveloperEvaluation.Application.Common.Carts;
using Ambev.DeveloperEvaluation.Common.Pagination;
using Ambev.DeveloperEvaluation.Domain.Enums;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Queries.Carts.SearchCarts;

/// <summary>
/// Contains params to get paginated cart and items
/// </summary>
public record SearchCartsQuery : PaginationParams, IRequest<PaginatedResponse<CartResult>>
{
    public Guid? CustomerId { get; init; }
    public Guid? BranchId { get; init; }
    public CartStatus? Status { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }

    [System.Text.Json.Serialization.JsonConstructor]
    public SearchCartsQuery(
        int page = 1,
        int pageSize = 10,
        string? orderBy = null,
        Guid? customerId = null,
        Guid? branchId = null,
        CartStatus? status = null,
        DateTime? startDate = null,
        DateTime? endDate = null)
        : base(page, pageSize, orderBy)
    {
        CustomerId = customerId;
        BranchId = branchId;
        Status = status;
        StartDate = startDate;
        EndDate = endDate;
    }
}