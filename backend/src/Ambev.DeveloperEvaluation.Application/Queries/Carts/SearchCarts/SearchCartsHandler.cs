using Ambev.DeveloperEvaluation.Application.Common.Carts;
using Ambev.DeveloperEvaluation.Common.Pagination;
using Ambev.DeveloperEvaluation.Domain.Queries.Carts;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Queries.Carts.SearchCarts;

/// <summary>
/// Handler for processing SearchCartsQuery requests
/// </summary>
public class SearchCartsHandler
    : IRequestHandler<SearchCartsQuery, PaginatedResponse<CartResult>>
{
    private readonly ICartQuery _cartQuery;
    private readonly IMapper _mapper;
    private readonly ILogger<SearchCartsHandler> _logger;

    public SearchCartsHandler(
        ICartQuery cartQuery,
        IMapper mapper,
        ILogger<SearchCartsHandler> logger)
    {
        _cartQuery = cartQuery;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PaginatedResponse<CartResult>> Handle(
        SearchCartsQuery query,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Searching carts - Page: {Page}, Size: {Size}",
            query.Page,
            query.PageSize);

        var filter = _mapper.Map<CartSearchFilter>(query);

        var pagedReadModels = await _cartQuery.SearchAsync(filter, cancellationToken);

        _logger.LogInformation(
            "Retrieved {Count} carts of {Total}",
            pagedReadModels.Data.Count,
            pagedReadModels.TotalCount);

        var results = _mapper.Map<List<CartResult>>(pagedReadModels.Data);

        return new PaginatedResponse<CartResult>(
            results,
            pagedReadModels.CurrentPage,
            pagedReadModels.TotalPages,
            pagedReadModels.TotalCount
        );
    }
}
