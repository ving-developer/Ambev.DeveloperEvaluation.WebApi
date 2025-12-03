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
public class CreateCartCommandHandler : IRequestHandler<CreateCartCommand, CartResult>
{
    private readonly ICartRepository _cartRepository;
    private readonly IUserRepository _userRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateCartCommandHandler> _logger;

    public CreateCartCommandHandler(
        ICartRepository cartRepository,
        IUserRepository userRepository,
        IBranchRepository branchRepository,
        IProductRepository productRepository,
        IMapper mapper,
        ILogger<CreateCartCommandHandler> logger)
    {
        _cartRepository = cartRepository;
        _userRepository = userRepository;
        _branchRepository = branchRepository;
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<CartResult> Handle(CreateCartCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating new cart for customer {CustomerId}", command.CustomerId);

        var customer = await _userRepository.GetByIdAsync(command.CustomerId, cancellationToken)
            ?? throw new KeyNotFoundException($"Customer {command.CustomerId} not found");

        var branch = await _branchRepository.GetByIdAsync(command.BranchId, cancellationToken)
            ?? throw new KeyNotFoundException($"Branch {command.BranchId} not found");

        var cart = new Cart(command.CustomerId, command.BranchId, command.SaleNumber);

        foreach (var item in command.Items)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId, cancellationToken)
                ?? throw new InvalidOperationException($"Product {item.ProductId} not found");

            cart.AddItem(item.ProductId, item.Quantity, item.UnitPrice);
        }

        cart.Complete();

        await _cartRepository.CreateAsync(cart, cancellationToken);
        await _cartRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Cart {CartId} created successfully with {ItemCount} items",
            cart.Id, cart.Items.Count);

        return _mapper.Map<CartResult>(cart);
    }
}