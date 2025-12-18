using Ambev.DeveloperEvaluation.Application.Commands.Carts.CancelCart;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Carts.CancelCart;

public class CancelCartHandlerTests
{
    private readonly ICartRepository _cartRepository;
    private readonly ILogger<CancelCartHandler> _logger;
    private readonly IMediator _mediator;
    private readonly CancelCartHandler _handler;

    public CancelCartHandlerTests()
    {
        _cartRepository = Substitute.For<ICartRepository>();
        _logger = Substitute.For<ILogger<CancelCartHandler>>();
        _mediator = Substitute.For<IMediator>();
        _handler = new CancelCartHandler(_cartRepository, _logger, _mediator);
    }

    [Fact(DisplayName = "Cancel cart successfully → returns Unit")]
    public async Task Handle_ValidCommand_CancelsCart()
    {
        // Given
        var cart = CartTestData.GenerateValidCart();
        var command = new CancelCartCommand(cart.Id, "Customer requested cancellation");

        _cartRepository.GetByIdAsync(cart.Id, Arg.Any<CancellationToken>())
            .Returns(cart);

        _cartRepository.UpdateAsync(cart, Arg.Any<CancellationToken>())
            .Returns(cart);

        _cartRepository.SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(true));

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        cart.Status.Should().Be(CartStatus.Canceled);
        cart.CancellationReason.Should().Be(command.Reason);
        await _cartRepository.Received(1).GetByIdAsync(cart.Id, Arg.Any<CancellationToken>());
        await _cartRepository.Received(1).UpdateAsync(cart, Arg.Any<CancellationToken>());
        await _cartRepository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Cart not found → throws EntityNotFoundException")]
    public async Task Handle_CartNotFound_ThrowsEntityNotFoundException()
    {
        // Given
        var cartId = Guid.NewGuid();
        var command = new CancelCartCommand(cartId, "Reason");

        _cartRepository.GetByIdAsync(cartId, Arg.Any<CancellationToken>())
            .Returns((Cart?)null);

        // When
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should()
            .ThrowAsync<EntityNotFoundException>();
    }

    [Fact(DisplayName = "Cancellation reason is empty → throws DomainException")]
    public async Task Handle_EmptyReason_ThrowsDomainException()
    {
        // Given
        var cart = CartTestData.GenerateValidCart();
        var command = new CancelCartCommand(cart.Id, "");

        // When
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should()
            .ThrowAsync<DomainException>()
            .WithMessage("Cancellation reason is required");
    }

    [Fact(DisplayName = "Cancel already canceled cart → throws DomainException")]
    public async Task Handle_CancelAlreadyCanceledCart_ThrowsDomainException()
    {
        // Given
        var cart = CartTestData.GenerateCanceledCart("Initial reason");
        var command = new CancelCartCommand(cart.Id, "New cancellation reason");

        _cartRepository.GetByIdAsync(cart.Id, Arg.Any<CancellationToken>())
            .Returns(cart);

        // When
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should()
            .ThrowAsync<DomainException>()
            .WithMessage("Sale is already canceled.");
    }
}
