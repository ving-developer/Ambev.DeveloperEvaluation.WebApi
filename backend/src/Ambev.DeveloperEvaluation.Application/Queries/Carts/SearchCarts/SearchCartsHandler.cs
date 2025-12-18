using Ambev.DeveloperEvaluation.Application.Common.Carts;
using Ambev.DeveloperEvaluation.Common.Pagination;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Queries.Carts.SearchCarts;

/// <summary>
/// Handler for processing SearchCartsQuery requests
/// </summary>
public class SearchCartsHandler : IRequestHandler<SearchCartsQuery, PaginatedResponse<CartResult>>
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<SearchCartsHandler> _logger;

    public SearchCartsHandler(
        ICartRepository cartRepository,
        IMapper mapper,
        ILogger<SearchCartsHandler> logger)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PaginatedResponse<CartResult>> Handle(SearchCartsQuery command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Listing carts - Page: {Page}, Size: {PageSize}",
            command.Page, command.PageSize);

        var paginatedCarts = await _cartRepository.GetPaginatedAsync(
            predicate: BuildPredicate(command),
            pageNumber: command.Page,
            pageSize: command.PageSize,
            orderBy: command.OrderBy,
            cancellationToken: cancellationToken
        );

        _logger.LogInformation("Retrieved {Count} carts of {Total}",
            paginatedCarts.Data.Count, paginatedCarts.TotalCount);

        var cartResults = _mapper.Map<List<CartResult>>(paginatedCarts.Data);

        return new PaginatedResponse<CartResult>(
            cartResults,
            paginatedCarts.CurrentPage,
            paginatedCarts.TotalPages,
            paginatedCarts.TotalCount
        );
    }

    private static System.Linq.Expressions.Expression<Func<Cart, bool>>? BuildPredicate(SearchCartsQuery command)
    {
        if (!command.CustomerId.HasValue &&
            !command.BranchId.HasValue &&
            !command.Status.HasValue &&
            !command.StartDate.HasValue &&
            !command.EndDate.HasValue)
        {
            return null;
        }

        return cart =>
            (!command.CustomerId.HasValue || cart.CustomerId == command.CustomerId.Value) &&
            (!command.BranchId.HasValue || cart.BranchId == command.BranchId.Value) &&
            (!command.Status.HasValue || cart.Status == command.Status.Value) &&
            (!command.StartDate.HasValue || cart.CreatedAt >= command.StartDate.Value) &&
            (!command.EndDate.HasValue || cart.CreatedAt <= command.EndDate.Value);
    }
}