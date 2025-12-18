using Ambev.DeveloperEvaluation.Application.Common.Products;
using Ambev.DeveloperEvaluation.Common.Pagination;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Ambev.DeveloperEvaluation.Application.Queries.Product.SearchProducts;

/// <summary>
/// Handler for processing SearchProductsQuery requests
/// </summary>
public class SearchProductsHandler : IRequestHandler<SearchProductsQuery, PaginatedResponse<ProductResult>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<SearchProductsHandler> _logger;

    public SearchProductsHandler(
        IProductRepository productRepository,
        IMapper mapper,
        ILogger<SearchProductsHandler> logger)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PaginatedResponse<ProductResult>> Handle(SearchProductsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[INF] Starting ListProductsHandler. Page={Page}, PageSize={PageSize}",
            request.Page, request.PageSize);

        Expression<Func<Domain.Entities.Product, bool>>? predicate = null;

        var paginatedProducts = await _productRepository.GetPaginatedAsync(
            predicate,
            request.Page,
            request.PageSize,
            request.OrderBy,
            cancellationToken
        );

        var data = _mapper.Map<List<ProductResult>>(paginatedProducts.Data);

        _logger.LogInformation("[INF] Products retrieved successfully. Count={Count}", data.Count);

        return new PaginatedResponse<ProductResult>(
            data,
            paginatedProducts.CurrentPage,
            paginatedProducts.TotalPages,
            paginatedProducts.TotalCount);
    }
}