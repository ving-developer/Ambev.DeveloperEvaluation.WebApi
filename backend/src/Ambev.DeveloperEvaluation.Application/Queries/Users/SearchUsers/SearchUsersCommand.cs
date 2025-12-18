using Ambev.DeveloperEvaluation.Application.Common.Users;
using Ambev.DeveloperEvaluation.Common.Pagination;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Queries.Users.SearchUsers;

/// <summary>
/// Query for retrieving a paginated list of users
/// </summary>
public record SearchUsersCommand : PaginationParams, IRequest<PaginatedResponse<UserResult>>
{
    /// <summary>
    /// Initializes a new instance of ListUserCommand
    /// </summary>
    /// <param name="page">Page number</param>
    /// <param name="pageSize">Number of items per page</param>
    /// <param name="orderBy">Optional ordering</param>
    public SearchUsersCommand(int page = 1, int pageSize = 10, string? orderBy = null)
        : base(page, pageSize, orderBy) { }
}
