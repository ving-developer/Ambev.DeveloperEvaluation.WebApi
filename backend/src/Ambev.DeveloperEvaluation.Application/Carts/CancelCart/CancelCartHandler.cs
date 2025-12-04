using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Carts.CancelCart;

/// <summary>
/// Handler for processing CancelCartCommand requests
/// </summary>
public class CancelCartHandler : IRequestHandler<CancelCartCommand, Unit>
{
    private readonly ICartRepository _cartRepository;
    private readonly ILogger<CancelCartHandler> _logger;

    public CancelCartHandler(
        ICartRepository cartRepository,
        ILogger<CancelCartHandler> logger)
    {
        _cartRepository = cartRepository;
        _logger = logger;
    }

    public async Task<Unit> Handle(CancelCartCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Cancelling cart {CartId}", command.CartId);

        if (string.IsNullOrWhiteSpace(command.Reason))
            throw new DomainException("Cancellation reason is required");

        var cart = await _cartRepository.GetByIdAsync(command.CartId, cancellationToken) ?? throw new EntityNotFoundException(nameof(Cart), command.CartId);

        cart.Cancel(command.Reason);

        await _cartRepository.UpdateAsync(cart, cancellationToken);
        await _cartRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Cart {CartId} cancelled successfully", command.CartId);

        return Unit.Value;
    }
}