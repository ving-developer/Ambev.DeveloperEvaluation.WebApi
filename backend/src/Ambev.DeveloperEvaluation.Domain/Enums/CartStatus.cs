namespace Ambev.DeveloperEvaluation.Domain.Enums;

/// <summary>
/// Represents the status of a sale.
/// </summary>
public enum CartStatus
{
    /// <summary>
    /// Sale is pending completion.
    /// </summary>
    Pending = 1,

    /// <summary>
    /// Sale has been completed successfully.
    /// </summary>
    Completed = 2,

    /// <summary>
    /// Sale has been canceled.
    /// </summary>
    Canceled = 3
}