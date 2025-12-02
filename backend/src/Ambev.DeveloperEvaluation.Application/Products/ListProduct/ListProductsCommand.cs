using Ambev.DeveloperEvaluation.Application.Products.Common;
using Ambev.DeveloperEvaluation.Common.Pagination;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.ListProducts;

/// <summary>
/// Query for retrieving a paginated list of products
/// </summary>
public record ListProductsCommand : PaginationParams, IRequest<PaginatedResponse<ProductResult>>
{
    /// <summary>
    /// Initializes a new instance of ListProductsCommand
    /// </summary>
    /// <param name="page">Page number</param>
    /// <param name="pageSize">Number of items per page</param>
    /// <param name="orderBy">Optional ordering</param>
    public ListProductsCommand(int page = 1, int pageSize = 10, string? orderBy = null)
    {
        Page = page;
        PageSize = pageSize;
        OrderBy = orderBy;
    }
}