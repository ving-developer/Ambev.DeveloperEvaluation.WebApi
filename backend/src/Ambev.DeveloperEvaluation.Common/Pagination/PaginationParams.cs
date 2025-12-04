namespace Ambev.DeveloperEvaluation.Common.Pagination;

/// <summary>
/// Parameters for paginated queries
/// </summary>
public record PaginationParams
{
    /// <summary>
    /// Page number (default: 1)
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Number of items per page (default: 10)
    /// </summary>
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// Sorting order, e.g., "username asc, email desc"
    /// </summary>
    public string? OrderBy { get; set; }

    public PaginationParams(int page, int pageSize, string? orderBy)
    {
        Page = page;
        PageSize = pageSize;
        OrderBy = orderBy;
    }
}
