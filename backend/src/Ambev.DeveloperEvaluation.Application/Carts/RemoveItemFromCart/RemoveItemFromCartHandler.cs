using Ambev.DeveloperEvaluation.Application.Carts.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Carts.RemoveItemFromCart;

/// <summary>
/// Handler for processing RemoveItemFromCartCommand requests
/// </summary>
public class RemoveItemFromCartHandler : IRequestHandler<RemoveItemFromCartCommand, CartResult>
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<RemoveItemFromCartHandler> _logger;

    public RemoveItemFromCartHandler(
        ICartRepository cartRepository,
        IMapper mapper,
        ILogger<RemoveItemFromCartHandler> logger)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<CartResult> Handle(RemoveItemFromCartCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Removing item {ItemId} from cart {CartId}",
            command.ItemId, command.CartId);

        var cart = await _cartRepository.GetByIdAsync(command.CartId, cancellationToken) ?? throw new KeyNotFoundException($"Cart {command.CartId} not found");
        
        cart.RemoveItem(command.ItemId);

        await _cartRepository.UpdateAsync(cart, cancellationToken);
        await _cartRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Item {ItemId} removed from cart {CartId} successfully",
            command.ItemId, command.CartId);

        return _mapper.Map<CartResult>(cart);
    }
}