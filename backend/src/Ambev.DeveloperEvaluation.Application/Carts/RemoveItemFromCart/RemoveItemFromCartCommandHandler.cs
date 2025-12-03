using Ambev.DeveloperEvaluation.Application.Carts.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Carts.RemoveItemFromCart;

/// <summary>
/// Handler for processing RemoveItemFromCartCommand requests
/// </summary>
public class RemoveItemFromCartCommandHandler : IRequestHandler<RemoveItemFromCartCommand, CartResult>
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<RemoveItemFromCartCommandHandler> _logger;

    public RemoveItemFromCartCommandHandler(
        ICartRepository cartRepository,
        IMapper mapper,
        ILogger<RemoveItemFromCartCommandHandler> logger)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<CartResult> Handle(RemoveItemFromCartCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Removing item {ItemId} from cart {CartId}",
            command.ItemId, command.CartId);

        var cart = await _cartRepository.GetByIdAsync(command.CartId, cancellationToken);
        if (cart == null)
            throw new KeyNotFoundException($"Cart {command.CartId} not found");

        // Remover item (a entidade valida se pode remover)
        cart.RemoveItem(command.ItemId);

        await _cartRepository.UpdateAsync(cart, cancellationToken);
        await _cartRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Item {ItemId} removed from cart {CartId} successfully",
            command.ItemId, command.CartId);

        return _mapper.Map<CartResult>(cart);
    }
}