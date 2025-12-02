using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Common;

/// <summary>
/// Parameters for paginated queries
/// </summary>
public record PaginationQueryParams
{
    /// <summary>
    /// Page number (default: 1)
    /// </summary>
    [FromQuery(Name = "_page")]
    public int Page { get; set; } = 1;

    /// <summary>
    /// Number of items per page (default: 10)
    /// </summary>
    [FromQuery(Name = "_size")]
    public int Size { get; set; } = 10;

    /// <summary>
    /// Sorting order, e.g., "username asc, email desc"
    /// </summary>
    [FromQuery(Name = "_order")]
    public string? Order { get; set; }
}
