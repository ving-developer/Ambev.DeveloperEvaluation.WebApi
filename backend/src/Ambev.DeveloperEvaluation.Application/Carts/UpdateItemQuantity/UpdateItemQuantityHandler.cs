using Ambev.DeveloperEvaluation.Application.Carts.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateItemQuantity;

/// <summary>
/// Handler for processing UpdateItemQuantityCommand requests
/// </summary>
public class UpdateItemQuantityHandler : IRequestHandler<UpdateItemQuantityCommand, CartResult>
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateItemQuantityHandler> _logger;

    public UpdateItemQuantityHandler(
        ICartRepository cartRepository,
        IMapper mapper,
        ILogger<UpdateItemQuantityHandler> logger)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<CartResult> Handle(UpdateItemQuantityCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating quantity for item {ItemId} in cart {CartId} to {NewQuantity}",
            command.ItemId, command.CartId, command.Quantity);

        var cart = await _cartRepository.GetByIdAsync(command.CartId, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(Cart), command.CartId);

        cart.UpdateItemQuantity(command.ItemId, command.Quantity);

        await _cartRepository.UpdateAsync(cart, cancellationToken);
        await _cartRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Item {ItemId} quantity has been updated successfully", command.ItemId);

        return _mapper.Map<CartResult>(cart);
    }
}