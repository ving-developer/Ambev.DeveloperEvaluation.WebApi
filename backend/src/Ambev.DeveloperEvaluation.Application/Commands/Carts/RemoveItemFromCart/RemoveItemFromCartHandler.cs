using Ambev.DeveloperEvaluation.Application.Common.Carts;
using Ambev.DeveloperEvaluation.Application.Events.Carts.ItemCancelled;
using Ambev.DeveloperEvaluation.Application.Events.Carts.SaleModified;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Commands.Carts.RemoveItemFromCart;

/// <summary>
/// Handler for processing RemoveItemFromCartCommand requests
/// </summary>
public class RemoveItemFromCartHandler : IRequestHandler<RemoveItemFromCartCommand, CartResult>
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<RemoveItemFromCartHandler> _logger;
    private readonly IMediator _mediator;

    public RemoveItemFromCartHandler(
        ICartRepository cartRepository,
        IMapper mapper,
        ILogger<RemoveItemFromCartHandler> logger,
        IMediator mediator)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;
        _logger = logger;
        _mediator = mediator;
    }

    public async Task<CartResult> Handle(RemoveItemFromCartCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Removing item {ItemId} from cart {CartId}",
            command.ItemId, command.CartId);

        var cart = await _cartRepository.GetByIdAsync(command.CartId, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(Cart), command.CartId);

        var removedItem = cart.RemoveItem(command.ItemId);

        await _cartRepository.UpdateAsync(cart, cancellationToken);
        await _cartRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Publishing SaleModified Notification Message.");

        await _mediator.Publish(
            new ItemCancelledNotification(new ItemCancelledEvent(cart, removedItem)),
            cancellationToken
        );

        _logger.LogInformation("Item {ItemId} removed from cart {CartId} successfully",
            command.ItemId, command.CartId);

        return _mapper.Map<CartResult>(cart);
    }
}