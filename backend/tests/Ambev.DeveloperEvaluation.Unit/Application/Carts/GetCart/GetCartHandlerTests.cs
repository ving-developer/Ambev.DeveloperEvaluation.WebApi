using Ambev.DeveloperEvaluation.Application.Carts.Common;
using Ambev.DeveloperEvaluation.Application.Carts.GetCart;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Carts.GetCart;

public class GetCartHandlerTests
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;
    private readonly GetCartHandler _handler;

    public GetCartHandlerTests()
    {
        _cartRepository = Substitute.For<ICartRepository>();

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Cart, CartResult>();
            cfg.CreateMap<CartItem, CartItemCommand>();
        });

        _mapper = mapperConfig.CreateMapper();

        _handler = new GetCartHandler(
            _cartRepository,
            _mapper,
            Substitute.For<ILogger<GetCartHandler>>());
    }

    [Fact(DisplayName = "Get cart successfully → returns CartResult")]
    public async Task Handle_ValidCartId_ReturnsCartResult()
    {
        // Given
        var cart = CartTestData.GeneratePendingCart();
        _cartRepository.GetByIdAsync(cart.Id, Arg.Any<CancellationToken>())
            .Returns(cart);

        var command = new GetCartCommand(cart.Id);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Id.Should().Be(cart.Id);
        result.CustomerId.Should().Be(cart.CustomerId);
        result.Items.Count.Should().Be(cart.Items.Count);

        await _cartRepository.Received(1).GetByIdAsync(cart.Id, Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Cart not found → throws EntityNotFoundException")]
    public async Task Handle_CartNotFound_ThrowsEntityNotFoundException()
    {
        // Given
        var cartId = Guid.NewGuid();
        _cartRepository.GetByIdAsync(cartId, Arg.Any<CancellationToken>())
            .Returns((Cart?)null);

        var command = new GetCartCommand(cartId);

        // When
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should()
            .ThrowAsync<EntityNotFoundException>();

        await _cartRepository.Received(1).GetByIdAsync(cartId, Arg.Any<CancellationToken>());
    }
}
