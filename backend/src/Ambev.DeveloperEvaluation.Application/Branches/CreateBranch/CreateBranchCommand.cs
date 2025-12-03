using Ambev.DeveloperEvaluation.Application.Branches.Common;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branches.CreateBranch;

/// <summary>
/// Command for creating a new branch.
/// </summary>
/// <remarks>
/// This command is used to capture the required data for creating a branch, 
/// including name, code, city, and state. 
/// It implements <see cref="IRequest{TResponse}"/> to initiate the request 
/// that returns a <see cref="BranchResult"/>.
/// 
/// The data provided in this command is validated using the 
/// <see cref="CreateBranchValidator"/> which extends 
/// <see cref="AbstractValidator{T}"/> to ensure that the fields are correctly 
/// populated and follow the required rules.
/// </remarks>
public record CreateBranchCommand : IRequest<BranchResult>
{
    /// <summary>
    /// Gets or sets the name of the branch to be created.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the code for the branch.
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the city where the branch is located.
    /// </summary>
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the state where the branch is located.
    /// </summary>
    public string State { get; set; } = string.Empty;
}