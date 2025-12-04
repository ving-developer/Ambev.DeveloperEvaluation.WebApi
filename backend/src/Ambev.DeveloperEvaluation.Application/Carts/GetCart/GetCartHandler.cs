using Ambev.DeveloperEvaluation.Application.Carts.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCart;

/// <summary>
/// Handler for processing GetCartCommand requests
/// </summary>
public class GetCartHandler : IRequestHandler<GetCartCommand, CartResult>
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetCartHandler> _logger;

    public GetCartHandler(
        ICartRepository cartRepository,
        IMapper mapper,
        ILogger<GetCartHandler> logger)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<CartResult> Handle(GetCartCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving cart {CartId}", command.CartId);

        var cart = await _cartRepository.GetByIdAsync(command.CartId, cancellationToken);
        if (cart == null)
        {
            _logger.LogWarning("Cart {CartId} not found", command.CartId);
            throw new KeyNotFoundException($"Cart {command.CartId} not found");
        }

        _logger.LogInformation("Cart {CartId} retrieved successfully", command.CartId);

        return _mapper.Map<CartResult>(cart);
    }
}