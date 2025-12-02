using Ambev.DeveloperEvaluation.Application.Users.Common;
using Ambev.DeveloperEvaluation.Common.Pagination;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Ambev.DeveloperEvaluation.Application.Users.ListUser;

/// <summary>
/// Handler for processing ListUserCommand requests
/// </summary>
public class ListUserHandler : IRequestHandler<ListUserCommand, PaginatedResponse<UserResult>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ListUserHandler> _logger;

    public ListUserHandler(
        IUserRepository userRepository,
        IMapper mapper,
        ILogger<ListUserHandler> logger)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PaginatedResponse<UserResult>> Handle(ListUserCommand request, CancellationToken cancellationToken)
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
