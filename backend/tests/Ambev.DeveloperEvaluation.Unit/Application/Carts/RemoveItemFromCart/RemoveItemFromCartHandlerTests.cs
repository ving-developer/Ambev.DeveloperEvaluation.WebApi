using Ambev.DeveloperEvaluation.Application.Carts.Common;
using Ambev.DeveloperEvaluation.Application.Carts.RemoveItemFromCart;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Carts.RemoveItemFromCart;

public class RemoveItemFromCartHandlerTests
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;
    private readonly RemoveItemFromCartHandler _handler;

    public RemoveItemFromCartHandlerTests()
    {
        _cartRepository = Substitute.For<ICartRepository>();

        var mapperConfig = new MapperConfiguration(cfg => cfg.CreateMap<Cart, CartResult>());
        _mapper = mapperConfig.CreateMapper();

        _handler = new RemoveItemFromCartHandler(
            _cartRepository,
            _mapper,
            Substitute.For<ILogger<RemoveItemFromCartHandler>>());
    }

    [Fact(DisplayName = "Handle valid request → removes item from cart")]
    public async Task Handle_ValidRequest_RemovesItem()
    {
        // Given
        var cart = CartTestData.GeneratePendingCart();
        var item = cart.Items.First();

        _cartRepository
            .GetByIdAsync(cart.Id, Arg.Any<CancellationToken>())
            .Returns(cart);

        var command = new RemoveItemFromCartCommand(cart.Id, item.Id);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Items.Should().NotContain(i => i.Id == item.Id);

        await _cartRepository.Received(1).GetByIdAsync(cart.Id, Arg.Any<CancellationToken>());
        await _cartRepository.Received(1).UpdateAsync(cart, Arg.Any<CancellationToken>());
        await _cartRepository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Handle non-existent cart → throws KeyNotFoundException")]
    public async Task Handle_CartNotFound_ThrowsKeyNotFoundException()
    {
        // Given
        var cartId = Guid.NewGuid();
        _cartRepository.GetByIdAsync(cartId, Arg.Any<CancellationToken>()).Returns((Cart)null);

        var command = new RemoveItemFromCartCommand(cartId, Guid.NewGuid());

        // When
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Cart {cartId} not found");

        await _cartRepository.Received(1).GetByIdAsync(cartId, Arg.Any<CancellationToken>());
        await _cartRepository.DidNotReceive().UpdateAsync(Arg.Any<Cart>(), Arg.Any<CancellationToken>());
        await _cartRepository.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Handle removing non-existent item → throws ArgumentException")]
    public async Task Handle_ItemNotFound_ThrowsArgumentException()
    {
        // Given
        var cart = CartTestData.GeneratePendingCart();

        _cartRepository
            .GetByIdAsync(cart.Id, Arg.Any<CancellationToken>())
            .Returns(cart);

        var command = new RemoveItemFromCartCommand(cart.Id, Guid.NewGuid());

        // When
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage($"Item {command.ItemId} not found in cart.*");

        await _cartRepository.Received(1).GetByIdAsync(cart.Id, Arg.Any<CancellationToken>());
        await _cartRepository.DidNotReceive().UpdateAsync(Arg.Any<Cart>(), Arg.Any<CancellationToken>());
        await _cartRepository.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
