using Ambev.DeveloperEvaluation.Application.Branches.Common;
using Ambev.DeveloperEvaluation.Common.Pagination;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Ambev.DeveloperEvaluation.Application.Branches.ListBranches;

/// <summary>
/// Handler for processing ListBranchesCommand requests
/// </summary>
public class ListBranchesHandler : IRequestHandler<ListBranchesCommand, PaginatedResponse<BranchResult>>
{
    private readonly IBranchRepository _branchRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ListBranchesHandler> _logger;

    public ListBranchesHandler(
        IBranchRepository branchRepository,
        IMapper mapper,
        ILogger<ListBranchesHandler> logger)
    {
        _branchRepository = branchRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PaginatedResponse<BranchResult>> Handle(ListBranchesCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[INF] Starting ListBranchesHandler. Page={Page}, PageSize={PageSize}", request.Page, request.PageSize);

        Expression<Func<Domain.Entities.Branch, bool>>? predicate = null;

        var paginatedBranches = await _branchRepository.GetPaginatedAsync(
            predicate,
            request.Page,
            request.PageSize,
            request.OrderBy,
            cancellationToken
        );

        var data = _mapper.Map<List<BranchResult>>(paginatedBranches.Data);

        _logger.LogInformation("[INF] Branches retrieved successfully. Count={Count}", data.Count);

        return new PaginatedResponse<BranchResult>(
            data,
            paginatedBranches.CurrentPage,
            paginatedBranches.TotalPages,
            paginatedBranches.TotalCount);
    }
}