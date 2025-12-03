using Ambev.DeveloperEvaluation.Application.Carts.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Carts.AddItemToCart;

/// <summary>
/// Handler for processing AddItemToCartCommand requests
/// </summary>
public class AddItemToCartCommandHandler : IRequestHandler<AddItemToCartCommand, CartResult>
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<AddItemToCartCommandHandler> _logger;

    public AddItemToCartCommandHandler(
        ICartRepository cartRepository,
        IProductRepository productRepository,
        IMapper mapper,
        ILogger<AddItemToCartCommandHandler> logger)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<CartResult> Handle(AddItemToCartCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Adding item to cart {CartId}", command.CartId);

        var cart = await _cartRepository.GetByIdAsync(command.CartId, cancellationToken);
        if (cart == null)
            throw new KeyNotFoundException($"Cart {command.CartId} not found");

        // Verificar se produto existe
        var product = await _productRepository.GetByIdAsync(command.ProductId, cancellationToken);
        if (product == null)
            throw new KeyNotFoundException($"Product {command.ProductId} not found");

        // Adicionar item (a entidade aplica todas as regras de negócio)
        cart.AddItem(command.ProductId, command.Quantity, command.UnitPrice);

        // Se a venda estava completa, voltar para pendente? Decisão de negócio.
        // Por enquanto, assumimos que só se adiciona a vendas pendentes.
        if (cart.Status == Domain.Enums.CartStatus.Completed)
        {
            _logger.LogWarning("Adding item to completed cart {CartId} - consider business rules", command.CartId);
            // Talvez lançar exceção ou criar nova venda
        }

        await _cartRepository.UpdateAsync(cart, cancellationToken);
        await _cartRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Item added to cart {CartId} successfully", command.CartId);

        return _mapper.Map<CartResult>(cart);
    }
}