namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches.CreateBranch;

/// <summary>
/// Represents a request to create a new branch in the system.
/// </summary>
public record CreateBranchRequest
{
    /// <summary>
    /// Gets or sets the name of the branch. Must be between 2 and 100 characters.
    /// </summary>
    /// <example>São Paulo Centro</example>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unique code of the branch. Must follow format XX-001.
    /// </summary>
    /// <example>SP-001</example>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the city where the branch is located.
    /// </summary>
    /// <example>São Paulo</example>
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the state where the branch is located. Must be a 2-letter Brazilian state abbreviation.
    /// </summary>
    /// <example>SP</example>
    public string State { get; set; } = string.Empty;
}