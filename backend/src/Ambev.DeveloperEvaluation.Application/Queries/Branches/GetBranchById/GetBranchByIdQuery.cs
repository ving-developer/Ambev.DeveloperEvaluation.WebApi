using Ambev.DeveloperEvaluation.Application.Common.Branches;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Queries.Branches.GetBranchById;

/// <summary>
/// Command for retrieving a branch by its ID
/// </summary>
public record GetBranchByIdQuery : IRequest<BranchResult>
{
    /// <summary>
    /// The unique identifier of the branch to retrieve
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Initializes a new instance of GetBranchCommand
    /// </summary>
    /// <param name="id">The ID of the branch to retrieve</param>
    public GetBranchByIdQuery(Guid id)
    {
        Id = id;
    }
}