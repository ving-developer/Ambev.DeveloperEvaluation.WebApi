using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Carts.CancelCart;

/// <summary>
/// Handler for processing CancelCartCommand requests
/// </summary>
public class CancelCartCommandHandler : IRequestHandler<CancelCartCommand, Unit>
{
    private readonly ICartRepository _cartRepository;
    private readonly ILogger<CancelCartCommandHandler> _logger;

    public CancelCartCommandHandler(
        ICartRepository cartRepository,
        ILogger<CancelCartCommandHandler> logger)
    {
        _cartRepository = cartRepository;
        _logger = logger;
    }

    public async Task<Unit> Handle(CancelCartCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Cancelling cart {CartId}", command.CartId);

        var cart = await _cartRepository.GetByIdAsync(command.CartId, cancellationToken);
        if (cart == null)
            throw new KeyNotFoundException($"Cart {command.CartId} not found");

        if (string.IsNullOrWhiteSpace(command.Reason))
            throw new InvalidOperationException("Cancellation reason is required");

        // Aplicar regra de cancelamento (a entidade valida se já está cancelada)
        cart.Cancel(command.Reason);

        await _cartRepository.UpdateAsync(cart, cancellationToken);
        await _cartRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Cart {CartId} cancelled successfully", command.CartId);

        return Unit.Value;
    }
}