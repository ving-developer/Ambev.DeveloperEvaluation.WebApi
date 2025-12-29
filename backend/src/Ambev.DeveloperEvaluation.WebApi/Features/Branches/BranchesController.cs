using Ambev.DeveloperEvaluation.Application.Commands.Branches.CreateBranch;
using Ambev.DeveloperEvaluation.Application.Commands.Branches.DeleteBranch;
using Ambev.DeveloperEvaluation.Application.Queries.Branches.GetBranchById;
using Ambev.DeveloperEvaluation.Application.Queries.Branches.SearchBranches;
using Ambev.DeveloperEvaluation.Common.Pagination;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Branches.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Branches.CreateBranch;
using Ambev.DeveloperEvaluation.WebApi.Features.Branches.ListBranches;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches;

/// <summary>
/// Controller for managing branch operations
/// </summary>
[ApiController]
[Authorize]
[Route("api/[controller]")]
public class BranchesController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of BranchesController
    /// </summary>
    /// <param name="mediator">The mediator instance</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public BranchesController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Creates a new branch
    /// </summary>
    /// <param name="request">The branch creation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created branch details</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseWithData<BranchResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateBranch(
        [FromBody] CreateBranchRequest request,
        CancellationToken cancellationToken)
    {
        var command = _mapper.Map<CreateBranchCommand>(request);
        var response = await _mediator.Send(command, cancellationToken);
        var branchResponse = _mapper.Map<BranchResponse>(response);

        return Created("GetBranchById", new { id = branchResponse.Id }, branchResponse);
    }

    /// <summary>
    /// Retrieves a branch by its ID
    /// </summary>
    /// <param name="id">The unique identifier of the branch</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The branch details if found</returns>
    [HttpGet("{id}", Name = "GetBranchById")]
    [ProducesResponseType(typeof(ApiResponseWithData<BranchResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBranch(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var command = new GetBranchByIdQuery(id);
        var response = await _mediator.Send(command, cancellationToken);

        return Ok(_mapper.Map<BranchResponse>(response), "Branch retrieved successfully");
    }

    /// <summary>
    /// Retrieves a paginated list of branches
    /// </summary>
    /// <param name="request">Request params containing the search query params</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paged list of branches</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponse<BranchResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ListBranches(
        ListBranchesRequest request,
        CancellationToken cancellationToken)
    {
        var command = _mapper.Map<SearchBranchesQuery>(request);
        var response = await _mediator.Send(command, cancellationToken);

        return OkPaginated(response);
    }

    /// <summary>
    /// Deletes a branch by its ID
    /// </summary>
    /// <param name="id">The unique identifier of the branch to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success response if the branch was deleted</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteBranch(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteBranchCommand(id);
        await _mediator.Send(command, cancellationToken);

        return Ok(new ApiResponse
        {
            Success = true,
            Message = "Branch deleted successfully"
        });
    }
}