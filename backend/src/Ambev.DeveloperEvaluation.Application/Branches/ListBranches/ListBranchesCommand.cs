using Ambev.DeveloperEvaluation.Application.Branches.Common;
using Ambev.DeveloperEvaluation.Common.Pagination;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branches.ListBranches;

/// <summary>
/// Query for retrieving a paginated list of branches
/// </summary>
public record ListBranchesCommand : PaginationParams, IRequest<PaginatedResponse<BranchResult>>
{
    /// <summary>
    /// Initializes a new instance of ListBranchesCommand
    /// </summary>
    /// <param name="page">Page number</param>
    /// <param name="pageSize">Number of items per page</param>
    /// <param name="orderBy">Optional ordering</param>
    [System.Text.Json.Serialization.JsonConstructor]
    public ListBranchesCommand(int page = 1, int pageSize = 10, string? orderBy = null)
        : base(page, pageSize, orderBy) { }
}