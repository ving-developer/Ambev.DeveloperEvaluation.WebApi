using Ambev.DeveloperEvaluation.Application.Common.Branches;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Commands.Branches.CreateBranch;

/// <summary>
/// Handler for processing CreateBranchCommand requests
/// </summary>
public class CreateBranchHandler : IRequestHandler<CreateBranchCommand, BranchResult>
{
    private readonly IBranchRepository _branchRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateBranchHandler> _logger;

    /// <summary>
    /// Initializes a new instance of CreateBranchHandler
    /// </summary>
    /// <param name="branchRepository">The branch repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="logger">The logger instance</param>
    public CreateBranchHandler(
        IBranchRepository branchRepository,
        IMapper mapper,
        ILogger<CreateBranchHandler> logger)
    {
        _branchRepository = branchRepository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Handles the CreateBranchCommand request
    /// </summary>
    /// <param name="command">The CreateBranch command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created branch details</returns>
    public async Task<BranchResult> Handle(CreateBranchCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[INF] Starting CreateBranchHandler execution");

        var existingBranch = await _branchRepository.GetByCodeAsync(command.Code, cancellationToken);

        if (existingBranch != null)
        {
            _logger.LogWarning("[WRN] Branch with Code={Code} already exists. Aborting CreateBranch.", command.Code);
            throw new DomainException($"Branch with code {command.Code} already exists");
        }

        var branch = _mapper.Map<Branch>(command);

        _logger.LogInformation("[INF] Saving branch into repository");

        var createdBranch = await _branchRepository.CreateAsync(branch, cancellationToken);

        await _branchRepository.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("[INF] Branch (BranchId={BranchId}) created successfully", createdBranch.Id);

        return _mapper.Map<BranchResult>(createdBranch);
    }
}