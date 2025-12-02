namespace Ambev.DeveloperEvaluation.Common.Pagination;

/// <summary>
/// Represents a paginated response with metadata and data items
/// </summary>
/// <typeparam name="T">The type of items in the response</typeparam>
public record PaginatedResponse<T>
{
    /// <summary>
    /// The list of items for the current page
    /// </summary>
    public List<T> Data { get; set; } = new();

    /// <summary>
    /// Gets or sets the current page number
    /// </summary>
    public int CurrentPage { get; set; }

    /// <summary>
    /// Gets or sets the total number of pages available
    /// </summary>
    public int TotalPages { get; set; }

    /// <summary>
    /// Gets or sets the total number of items across all pages
    /// </summary>
    public int TotalCount { get; set; }

    public PaginatedResponse(List<T> data, int currentPage, int totalPages, int totalCount)
    {
        Data = data;
        CurrentPage = currentPage;
        TotalPages = totalPages;
        TotalCount = totalCount;
    }
}
