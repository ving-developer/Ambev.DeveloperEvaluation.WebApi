using Ambev.DeveloperEvaluation.Application.Products.Common;
using Ambev.DeveloperEvaluation.Application.Products.ListProducts;
using Ambev.DeveloperEvaluation.Common.Pagination;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Ambev.DeveloperEvaluation.Application.Products.ListProduct;

/// <summary>
/// Handler for processing ListProductsCommand requests
/// </summary>
public class ListProductsHandler : IRequestHandler<ListProductsCommand, PaginatedResponse<ProductResult>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ListProductsHandler> _logger;

    public ListProductsHandler(
        IProductRepository productRepository,
        IMapper mapper,
        ILogger<ListProductsHandler> logger)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PaginatedResponse<ProductResult>> Handle(ListProductsCommand request, CancellationToken cancellationToken)
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