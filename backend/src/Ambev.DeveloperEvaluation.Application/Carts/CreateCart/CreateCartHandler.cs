using Ambev.DeveloperEvaluation.Application.Carts.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart;

/// <summary>
/// Handler for processing CreateCartCommand requests
/// </summary>
public class CreateCartHandler : IRequestHandler<CreateCartCommand, CartResult>
{
    private readonly ICartRepository _cartRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IProductRepository _productRepository;
    private readonly ISaleCounterRepository _saleCounterRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateCartHandler> _logger;

    public CreateCartHandler(
        ICartRepository cartRepository,
        IBranchRepository branchRepository,
        IProductRepository productRepository,
        ISaleCounterRepository saleCounterRepository,
        IMapper mapper,
        ILogger<CreateCartHandler> logger)
    {
        _cartRepository = cartRepository;
        _branchRepository = branchRepository;
        _productRepository = productRepository;
        _saleCounterRepository = saleCounterRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<CartResult> Handle(CreateCartCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating new cart for customer {CustomerId} to branch: {BranchId}", command.CustomerId, command.BranchId);

        var branch = await _branchRepository.GetByIdAsync(command.BranchId, cancellationToken) ?? throw new KeyNotFoundException($"Branch with ID {command.BranchId} not found.");
        var saleCount = await _saleCounterRepository.GetAndIncrementSaleNumberAsync(command.BranchId, cancellationToken);
        var cart = new Cart(command.CustomerId, command.BranchId, $"{branch.Code}{saleCount:D6}");

        foreach (var item in command.Items)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId) ?? throw new KeyNotFoundException($"Product with ID {item.ProductId} not found.");
            cart.AddItem(item.ProductId, item.Quantity, product.Price);
        }

        await _cartRepository.CreateAsync(cart, cancellationToken);
        await _cartRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Cart {CartId} created successfully with {ItemCount} items",
            cart.Id, cart.Items.Count);

        return _mapper.Map<CartResult>(cart);
    }
}