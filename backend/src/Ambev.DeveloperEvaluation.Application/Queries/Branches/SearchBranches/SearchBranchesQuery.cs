using Ambev.DeveloperEvaluation.Application.Common.Branches;
using Ambev.DeveloperEvaluation.Common.Pagination;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Queries.Branches.SearchBranches;

/// <summary>
/// Query for retrieving a paginated list of branches
/// </summary>
public record SearchBranchesQuery : PaginationParams, IRequest<PaginatedResponse<BranchResult>>
{
    /// <summary>
    /// Initializes a new instance of ListBranchesCommand
    /// </summary>
    /// <param name="page">Page number</param>
    /// <param name="pageSize">Number of items per page</param>
    /// <param name="orderBy">Optional ordering</param>
    [System.Text.Json.Serialization.JsonConstructor]
    public SearchBranchesQuery(int page = 1, int pageSize = 10, string? orderBy = null)
        : base(page, pageSize, orderBy) { }
}