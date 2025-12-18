using Ambev.DeveloperEvaluation.Application.Common.Products;
using Ambev.DeveloperEvaluation.Common.Pagination;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Queries.Products.SearchProducts;

/// <summary>
/// Query for retrieving a paginated list of products
/// </summary>
public record SearchProductsQuery : PaginationParams, IRequest<PaginatedResponse<ProductResult>>
{
    /// <summary>
    /// Initializes a new instance of ListProductsCommand
    /// </summary>
    /// <param name="page">Page number</param>
    /// <param name="pageSize">Number of items per page</param>
    /// <param name="orderBy">Optional ordering</param>
    public SearchProductsQuery(int page = 1, int pageSize = 10, string? orderBy = null)
        : base(page, pageSize, orderBy) { }
}