namespace Ambev.DeveloperEvaluation.Application.Branches.Common;

/// <summary>
/// Response model for GetBranch operation
/// </summary>
public class BranchResult
{
    /// <summary>
    /// The unique identifier of the branch
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The name of the branch
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The unique code of the branch
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// The city where the branch is located
    /// </summary>
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// The state where the branch is located
    /// </summary>
    public string State { get; set; } = string.Empty;
}