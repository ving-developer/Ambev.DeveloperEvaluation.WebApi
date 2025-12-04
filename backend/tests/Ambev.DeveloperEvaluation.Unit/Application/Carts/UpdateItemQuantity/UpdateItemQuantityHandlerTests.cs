using Ambev.DeveloperEvaluation.Application.Carts.Common;
using Ambev.DeveloperEvaluation.Application.Carts.UpdateItemQuantity;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Carts.UpdateItemQuantity;

public class UpdateItemQuantityHandlerTests
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;
    private readonly UpdateItemQuantityHandler _handler;

    public UpdateItemQuantityHandlerTests()
    {
        _cartRepository = Substitute.For<ICartRepository>();

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Cart, CartResult>();
            cfg.CreateMap<CartItem, CartItemCommand>();
        });

        _mapper = mapperConfig.CreateMapper();

        _handler = new UpdateItemQuantityHandler(
            _cartRepository,
            _mapper,
            Substitute.For<ILogger<UpdateItemQuantityHandler>>());
    }

    [Fact(DisplayName = "Handle valid request → updates item quantity")]
    public async Task Handle_ValidRequest_UpdatesItemQuantity()
    {
        // Given
        var cart = CartTestData.GeneratePendingCart();
        var item = cart.Items.First();

        _cartRepository
            .GetByIdAsync(cart.Id, Arg.Any<CancellationToken>())
            .Returns(cart);

        var command = new UpdateItemQuantityCommand(cart.Id, item.Id, item.Quantity + 2);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        var updatedItem = result.Items.First(i => i.Id == item.Id);
        updatedItem.Quantity.Should().Be(command.Quantity);

        await _cartRepository.Received(1).GetByIdAsync(cart.Id, Arg.Any<CancellationToken>());
        await _cartRepository.Received(1).UpdateAsync(cart, Arg.Any<CancellationToken>());
        await _cartRepository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Handle non-existent cart → throws EntityNotFoundException")]
    public async Task Handle_CartNotFound_ThrowsEntityNotFoundException()
    {
        // Given
        var cartId = Guid.NewGuid();
        _cartRepository.GetByIdAsync(cartId, Arg.Any<CancellationToken>()).Returns((Cart)null);

        var command = new UpdateItemQuantityCommand(cartId, Guid.NewGuid(), 5);

        // When
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<EntityNotFoundException>();

        await _cartRepository.Received(1).GetByIdAsync(cartId, Arg.Any<CancellationToken>());
        await _cartRepository.DidNotReceive().UpdateAsync(Arg.Any<Cart>(), Arg.Any<CancellationToken>());
        await _cartRepository.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Handle updating non-existent item → throws InvalidOperationException or ArgumentException")]
    public async Task Handle_ItemNotFound_ThrowsException()
    {
        // Given
        var cart = CartTestData.GeneratePendingCart();

        _cartRepository
            .GetByIdAsync(cart.Id, Arg.Any<CancellationToken>())
            .Returns(cart);

        var command = new UpdateItemQuantityCommand(cart.Id, Guid.NewGuid(), 5);

        // When
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"Item {command.ItemId} not found*");

        await _cartRepository.Received(1).GetByIdAsync(cart.Id, Arg.Any<CancellationToken>());
        await _cartRepository.DidNotReceive().UpdateAsync(Arg.Any<Cart>(), Arg.Any<CancellationToken>());
        await _cartRepository.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
