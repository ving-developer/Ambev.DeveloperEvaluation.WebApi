using Ambev.DeveloperEvaluation.Application.Common.Carts;
using Ambev.DeveloperEvaluation.Application.Queries.Carts.GetCartById;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Queries.Carts;
using Ambev.DeveloperEvaluation.Domain.ReadModels.Carts;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Carts.GetCartById;

/// <summary>
/// Unit tests for <see cref="GetCartByIdHandler"/>
/// </summary>
public class GetCartHandlerTests
{
    private readonly ICartQuery _cartQuery;
    private readonly IMapper _mapper;
    private readonly GetCartByIdHandler _handler;

    public GetCartHandlerTests()
    {
        _cartQuery = Substitute.For<ICartQuery>();

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<CartReadModel, CartResult>();
            cfg.CreateMap<CartProductReadModel, CartItemCommand>();
        });

        _mapper = mapperConfig.CreateMapper();

        _handler = new GetCartByIdHandler(
            _cartQuery,
            _mapper,
            Substitute.For<ILogger<GetCartByIdHandler>>());
    }

    [Fact(DisplayName = "Get cart successfully → returns CartResult")]
    public async Task Handle_ValidCartId_ReturnsCartResult()
    {
        // Given
        var cartId = Guid.NewGuid();

        var cart = new CartReadModel
        {
            Id = cartId,
            CustomerId = Guid.NewGuid(),
            SaleNumber = "S001"
        };

        var cartItems = new List<CartProductReadModel>
            {
                new () { ProductId = Guid.NewGuid(), Quantity = 2 },
                new () { ProductId = Guid.NewGuid(), Quantity = 1 }
            };

        _cartQuery.GetByIdAsync(cartId, Arg.Any<CancellationToken>())
            .Returns(cart);

        _cartQuery.GetItemsAsync(cartId, Arg.Any<CancellationToken>())
            .Returns(cartItems);

        var command = new GetCartByIdQuery(cartId);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Id.Should().Be(cart.Id);
        result.CustomerId.Should().Be(cart.CustomerId);
        result.Items.Should().HaveCount(cartItems.Count);

        await _cartQuery.Received(1).GetByIdAsync(cartId, Arg.Any<CancellationToken>());
        await _cartQuery.Received(1).GetItemsAsync(cartId, Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Cart not found → throws EntityNotFoundException")]
    public async Task Handle_CartNotFound_ThrowsEntityNotFoundException()
    {
        // Given
        var cartId = Guid.NewGuid();

        _cartQuery.GetByIdAsync(cartId, Arg.Any<CancellationToken>())
            .Returns((CartReadModel?)null);

        var command = new GetCartByIdQuery(cartId);

        // When
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<EntityNotFoundException>();

        await _cartQuery.Received(1).GetByIdAsync(cartId, Arg.Any<CancellationToken>());
        await _cartQuery.Received(1).GetItemsAsync(cartId, Arg.Any<CancellationToken>());
    }
}