using Ambev.DeveloperEvaluation.Application.Common.Users;
using Ambev.DeveloperEvaluation.Common.Pagination;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Ambev.DeveloperEvaluation.Application.Queries.Users.SearchUsers;

/// <summary>
/// Handler for processing SearchUsersQuery requests
/// </summary>
public class SearchUsersHandler : IRequestHandler<SearchUsersQuery, PaginatedResponse<UserResult>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<SearchUsersHandler> _logger;

    public SearchUsersHandler(
        IUserRepository userRepository,
        IMapper mapper,
        ILogger<SearchUsersHandler> logger)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PaginatedResponse<UserResult>> Handle(SearchUsersQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[INF] Starting ListUserHandler. Page={Page}, PageSize={PageSize}", request.Page, request.PageSize);

        Expression<Func<Domain.Entities.User, bool>>? predicate = null;

        var paginatedUsers = await _userRepository.GetPaginatedAsync(
            predicate,
            request.Page,
            request.PageSize,
            request.OrderBy,
            cancellationToken
        );

        var data = _mapper.Map<List<UserResult>>(paginatedUsers.Data);

        _logger.LogInformation("[INF] Users retrieved successfully. Count={Count}", data.Count);

        return new PaginatedResponse<UserResult>(
            data,
            paginatedUsers.CurrentPage,
            paginatedUsers.TotalPages,
            paginatedUsers.TotalCount);
    }
}
