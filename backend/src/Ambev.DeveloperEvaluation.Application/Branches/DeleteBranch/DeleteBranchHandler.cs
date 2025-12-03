using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Branches.DeleteBranch;

/// <summary>
/// Handler for processing DeleteBranchCommand requests
/// </summary>
public class DeleteBranchHandler : IRequestHandler<DeleteBranchCommand, bool>
{
    private readonly IBranchRepository _branchRepository;
    private readonly ILogger<DeleteBranchHandler> _logger;

    /// <summary>
    /// Initializes a new instance of DeleteBranchHandler
    /// </summary>
    /// <param name="branchRepository">The branch repository</param>
    /// <param name="logger">The logger instance</param>
    public DeleteBranchHandler(
        IBranchRepository branchRepository,
        ILogger<DeleteBranchHandler> logger)
    {
        _branchRepository = branchRepository;
        _logger = logger;
    }

    /// <summary>
    /// Handles the DeleteBranchCommand request
    /// </summary>
    /// <param name="request">The DeleteBranch command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The result of the delete operation</returns>
    public async Task<bool> Handle(DeleteBranchCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[INF] Starting DeleteBranchHandler for BranchId={BranchId}", request.Id);

        var success = await _branchRepository.DeleteAsync(request.Id, cancellationToken);

        if (!success)
        {
            _logger.LogWarning("[WRN] Delete failed. BranchId={BranchId} not found", request.Id);
            throw new KeyNotFoundException($"Branch with ID {request.Id} not found");
        }

        _logger.LogInformation("[INF] Branch deleted successfully. BranchId={BranchId}", request.Id);

        return true;
    }
}