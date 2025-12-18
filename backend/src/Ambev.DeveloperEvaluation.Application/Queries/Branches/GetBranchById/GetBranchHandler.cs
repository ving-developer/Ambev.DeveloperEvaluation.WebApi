using Ambev.DeveloperEvaluation.Application.Common.Branches;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Queries.Branches.GetBranchById;

/// <summary>
/// Handler for processing GetBranchCommand requests
/// </summary>
public class GetBranchHandler : IRequestHandler<GetBranchCommand, BranchResult>
{
    private readonly IBranchRepository _branchRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetBranchHandler> _logger;

    /// <summary>
    /// Initializes a new instance of GetBranchHandler
    /// </summary>
    /// <param name="branchRepository">The branch repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="logger">The logger instance</param>
    public GetBranchHandler(
        IBranchRepository branchRepository,
        IMapper mapper,
        ILogger<GetBranchHandler> logger)
    {
        _branchRepository = branchRepository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Handles the GetBranchCommand request
    /// </summary>
    /// <param name="request">The GetBranch command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The branch details if found</returns>
    public async Task<BranchResult> Handle(GetBranchCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[INF] Starting GetBranchHandler for BranchId={BranchId}", request.Id);

        var branch = await _branchRepository.GetByIdAsync(request.Id, cancellationToken);
        if (branch == null)
        {
            _logger.LogWarning("[WRN] Branch not found. BranchId={BranchId}", request.Id);
            throw new EntityNotFoundException(nameof(Branch), request.Id);
        }

        _logger.LogInformation("[INF] Branch retrieved successfully. BranchId={BranchId}", request.Id);

        return _mapper.Map<BranchResult>(branch);
    }
}