using Ambev.DeveloperEvaluation.Application.Carts.AddItemToCart;
using Ambev.DeveloperEvaluation.Application.Carts.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Carts.AddItemToCart;

public class AddItemToCartHandlerTests
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly AddItemToCartHandler _handler;

    public AddItemToCartHandlerTests()
    {
        _cartRepository = Substitute.For<ICartRepository>();
        _productRepository = Substitute.For<IProductRepository>();
        _mediator = Substitute.For<IMediator>();

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Cart, CartResult>();
            cfg.CreateMap<CartItem, CartItemCommand>();
        });

        _mapper = mapperConfig.CreateMapper();

        _handler = new AddItemToCartHandler(
            _cartRepository,
            _productRepository,
            _mapper,
            Substitute.For<ILogger<AddItemToCartHandler>>(),
            _mediator);
    }

    [Fact(DisplayName = "Add item successfully → returns CartResult")]
    public async Task Handle_ValidCommand_AddsItemToCart()
    {
        // Given
        var cart = CartTestData.GenerateValidCart();
        cart.Id = Guid.NewGuid();

        var product = ProductTestData.GenerateValidProduct();
        product.Id = Guid.NewGuid();

        var command = new AddItemToCartCommand
        {
            CartId = cart.Id,
            ProductId = product.Id,
            Quantity = 2
        };

        _cartRepository.GetByIdAsync(cart.Id, Arg.Any<CancellationToken>())
            .Returns(cart);

        _productRepository.GetByIdAsync(product.Id, Arg.Any<CancellationToken>())
            .Returns(product);

        _cartRepository.UpdateAsync(cart, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(cart));

        _cartRepository.SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(true));

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Items.Should().ContainSingle(i => i.ProductId == product.Id && i.Quantity == command.Quantity);

        await _cartRepository.Received(1).GetByIdAsync(cart.Id, Arg.Any<CancellationToken>());
        await _productRepository.Received(1).GetByIdAsync(product.Id, Arg.Any<CancellationToken>());
        await _cartRepository.Received(1).UpdateAsync(cart, Arg.Any<CancellationToken>());
        await _cartRepository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Cart not found → throws EntityNotFoundException")]
    public async Task Handle_CartNotFound_ThrowsEntityNotFoundException()
    {
        // Given
        var command = new AddItemToCartCommand
        {
            CartId = Guid.NewGuid(),
            ProductId = Guid.NewGuid(),
            Quantity = 1
        };

        _cartRepository.GetByIdAsync(command.CartId, Arg.Any<CancellationToken>())
            .Returns((Cart?)null);

        // When
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should()
            .ThrowAsync<EntityNotFoundException>();
    }

    [Fact(DisplayName = "Product not found → throws EntityNotFoundException")]
    public async Task Handle_ProductNotFound_ThrowsEntityNotFoundException()
    {
        // Given
        var cart = CartTestData.GenerateValidCart();
        cart.Id = Guid.NewGuid();

        var command = new AddItemToCartCommand
        {
            CartId = cart.Id,
            ProductId = Guid.NewGuid(),
            Quantity = 1
        };

        _cartRepository.GetByIdAsync(cart.Id, Arg.Any<CancellationToken>())
            .Returns(cart);

        _productRepository.GetByIdAsync(command.ProductId, Arg.Any<CancellationToken>())
            .Returns((Product?)null);

        // When
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should()
            .ThrowAsync<EntityNotFoundException>();
    }

    [Fact(DisplayName = "Item already exists in cart → throws DomainException")]
    public async Task Handle_ItemAlreadyExists_ThrowsDomainException()
    {
        // Given
        var product = ProductTestData.GenerateValidProduct();
        product.Id = Guid.NewGuid();

        var cart = CartTestData.GenerateValidCart();
        cart.Id = Guid.NewGuid();
        cart.AddItem(product.Id, 1, product.Price);

        var command = new AddItemToCartCommand
        {
            CartId = cart.Id,
            ProductId = product.Id,
            Quantity = 2
        };

        _cartRepository.GetByIdAsync(cart.Id, Arg.Any<CancellationToken>())
            .Returns(cart);

        _productRepository.GetByIdAsync(product.Id, Arg.Any<CancellationToken>())
            .Returns(product);

        // When
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should()
            .ThrowAsync<DomainException>()
            .WithMessage($"O produto {product.Id} já existe no carrinho, tente atualizar a quantidade dele.");
    }
}
