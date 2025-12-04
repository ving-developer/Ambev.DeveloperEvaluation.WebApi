using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a branch/store location in the system.
/// Simple entity for denormalized branch information.
/// </summary>
public class Branch : BaseEntity
{
    /// <summary>
    /// Private property to hold the sales associated with this branch.
    /// </summary>
    private readonly List<Cart> _sales = new();

    /// <summary>
    /// Gets the name of the branch.
    /// </summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the branch code/identifier.
    /// </summary>
    public string Code { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the city where the branch is located.
    /// </summary>
    public string City { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the state where the branch is located.
    /// </summary>
    public string State { get; private set; } = string.Empty;

    /// <summary>
    /// Navigation property for sales at this branch.
    /// </summary>
    public IReadOnlyCollection<Cart> Sales => _sales;

    /// <summary>
    /// Initializes a new instance of the Branch class.
    /// </summary>
    public Branch(string name, string code, string city, string state)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Code = code ?? throw new ArgumentNullException(nameof(code));
        City = city ?? throw new ArgumentNullException(nameof(city));
        State = state ?? throw new ArgumentNullException(nameof(state));
    }

    /// <summary>
    /// Private constructor for EF Core.
    /// </summary>
    private Branch() { }
}