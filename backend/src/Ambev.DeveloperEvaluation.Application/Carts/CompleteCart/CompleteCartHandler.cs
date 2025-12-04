using Ambev.DeveloperEvaluation.Application.Carts.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Carts.CompleteCart;

/// <summary>
/// Handler for processing CompleteCartCommand requests
/// </summary>
public class CompleteCartHandler : IRequestHandler<CompleteCartCommand, CartResult>
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CompleteCartHandler> _logger;

    public CompleteCartHandler(
        ICartRepository cartRepository,
        IMapper mapper,
        ILogger<CompleteCartHandler> logger)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<CartResult> Handle(CompleteCartCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Completing cart {CartId}", command.CartId);

        var cart = await _cartRepository.GetByIdAsync(command.CartId, cancellationToken)
           ?? throw new EntityNotFoundException(nameof(Cart), command.CartId);

        cart.Complete();

        await _cartRepository.UpdateAsync(cart, cancellationToken);
        await _cartRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Cart {CartId} completed successfully with total {TotalAmount}",
            cart.Id, cart.TotalAmount);

        return _mapper.Map<CartResult>(cart);
    }
}