using Ambev.DeveloperEvaluation.Application.Users.Common;
using Ambev.DeveloperEvaluation.Common.Pagination;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Users.ListUser;

/// <summary>
/// Query for retrieving a paginated list of users
/// </summary>
public record ListUserCommand : PaginationParams, IRequest<PaginatedResponse<UserResult>>
{
    /// <summary>
    /// Initializes a new instance of ListUserCommand
    /// </summary>
    /// <param name="page">Page number</param>
    /// <param name="pageSize">Number of items per page</param>
    /// <param name="orderBy">Optional ordering</param>
    public ListUserCommand(int page = 1, int pageSize = 10, string? orderBy = null)
    {
        Page = page;
        PageSize = pageSize;
        OrderBy = orderBy;
    }
}
