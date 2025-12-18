using Ambev.DeveloperEvaluation.Application.Common.Carts;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Queries.Carts;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Queries.Carts.GetCartById;

/// <summary>
/// Handler for processing GetCartByIdQuery requests
/// </summary>
public class GetCartByIdHandler : IRequestHandler<GetCartByIdQuery, CartResult>
{
    private readonly ICartQuery _cartQuery;
    private readonly IMapper _mapper;
    private readonly ILogger<GetCartByIdHandler> _logger;

    public GetCartByIdHandler(
        ICartQuery cartQuery,
        IMapper mapper,
        ILogger<GetCartByIdHandler> logger)
    {
        _cartQuery = cartQuery;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<CartResult> Handle(GetCartByIdQuery query, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving cart {CartId}", query.CartId);

        var cart = await _cartQuery.GetByIdAsync(query.CartId, cancellationToken);
        var items = await _cartQuery.GetItemsAsync(query.CartId, cancellationToken);


        if (cart is null)
        {
            _logger.LogWarning("Cart {CartId} not found", query.CartId);
            throw new EntityNotFoundException("Cart", query.CartId);
        }

        _logger.LogInformation("Cart {CartId} retrieved successfully", query.CartId);

        var result = _mapper.Map<CartResult>(cart);

        result.Items = _mapper.Map<List<CartItemCommand>>(items);

        return result;
    }

}
