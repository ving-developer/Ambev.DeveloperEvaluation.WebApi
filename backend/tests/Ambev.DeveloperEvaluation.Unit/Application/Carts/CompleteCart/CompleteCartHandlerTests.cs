using Ambev.DeveloperEvaluation.Application.Carts.Common;
using Ambev.DeveloperEvaluation.Application.Carts.CompleteCart;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Carts.CompleteCart;

public class CompleteCartHandlerTests
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;
    private readonly CompleteCartHandler _handler;

    public CompleteCartHandlerTests()
    {
        _cartRepository = Substitute.For<ICartRepository>();

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<CartItem, CartItemCommand>();
            cfg.CreateMap<Cart, CartResult>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
        });

        _mapper = mapperConfig.CreateMapper();

        _handler = new CompleteCartHandler(
            _cartRepository,
            _mapper,
            Substitute.For<ILogger<CompleteCartHandler>>());
    }

    [Fact(DisplayName = "Complete cart successfully → returns CartResult")]
    public async Task Handle_ValidCommand_CompletesCart()
    {
        // Given
        var cart = CartTestData.GeneratePendingCart();
        cart.AddItem(Guid.NewGuid(), 2, 50m);
        var command = new CompleteCartCommand(cart.Id);

        _cartRepository.GetByIdAsync(cart.Id, Arg.Any<CancellationToken>())
            .Returns(cart);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Id.Should().Be(cart.Id);
        result.Status.Should().Be(cart.Status.ToString());
        result.TotalAmount.Should().Be(cart.TotalAmount);

        await _cartRepository.Received(1).UpdateAsync(cart, Arg.Any<CancellationToken>());
        await _cartRepository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Cart not found → throws KeyNotFoundException")]
    public async Task Handle_CartNotFound_ThrowsKeyNotFoundException()
    {
        // Given
        var command = new CompleteCartCommand(Guid.NewGuid());

        _cartRepository.GetByIdAsync(command.CartId, Arg.Any<CancellationToken>())
            .Returns((Cart)null!);

        // When
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should()
            .ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Cart {command.CartId} not found");
    }

    [Fact(DisplayName = "Completing empty cart → throws InvalidOperationException")]
    public async Task Handle_EmptyCart_ThrowsInvalidOperationException()
    {
        // Given
        var cart = CartTestData.GenerateCartWithItems(0);
        var command = new CompleteCartCommand(cart.Id);

        _cartRepository.GetByIdAsync(cart.Id, Arg.Any<CancellationToken>())
            .Returns(cart);

        // When
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("Cannot complete a sale without items.");
    }
}
