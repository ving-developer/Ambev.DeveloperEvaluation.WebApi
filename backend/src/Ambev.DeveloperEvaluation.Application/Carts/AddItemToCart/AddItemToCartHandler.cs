using Ambev.DeveloperEvaluation.Application.Carts.Common;
using Ambev.DeveloperEvaluation.Application.Events.Carts.SaleModified;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Carts.AddItemToCart;

/// <summary>
/// Handler for processing AddItemToCartCommand requests
/// </summary>
public class AddItemToCartHandler : IRequestHandler<AddItemToCartCommand, CartResult>
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<AddItemToCartHandler> _logger;
    private readonly IMediator _mediator;

    public AddItemToCartHandler(
        ICartRepository cartRepository,
        IProductRepository productRepository,
        IMapper mapper,
        ILogger<AddItemToCartHandler> logger,
        IMediator mediator)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
        _mediator = mediator;
    }

    public async Task<CartResult> Handle(AddItemToCartCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Adding item to cart {CartId}", command.CartId);

        var cart = await _cartRepository.GetByIdAsync(command.CartId, cancellationToken)
           ?? throw new EntityNotFoundException(nameof(Cart), command.CartId);

        var product = await _productRepository.GetByIdAsync(command.ProductId, cancellationToken)
                      ?? throw new EntityNotFoundException(nameof(Product), command.ProductId);

        var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == command.ProductId);
        
        if (existingItem is not null)
        {
            throw new DomainException($"O produto {command.ProductId} já existe no carrinho, tente atualizar a quantidade dele.");
        }

        cart.AddItem(command.ProductId, command.Quantity, product.Price);

        if (cart.Status == Domain.Enums.CartStatus.Completed)
        {
            _logger.LogWarning("Adding item to completed cart {CartId}", command.CartId);
        }

        await _cartRepository.UpdateAsync(cart, cancellationToken);
        await _cartRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Publishing SaleModified Notification Message.");

        await _mediator.Publish(new SaleModifiedNotification(new SaleModifiedEvent(cart)), cancellationToken);

        _logger.LogInformation("Item added to cart {CartId} successfully", command.CartId);

        return _mapper.Map<CartResult>(cart);
    }
}