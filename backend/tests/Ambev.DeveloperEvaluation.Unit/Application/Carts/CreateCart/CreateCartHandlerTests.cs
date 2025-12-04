using Ambev.DeveloperEvaluation.Application.Carts.Common;
using Ambev.DeveloperEvaluation.Application.Carts.CreateCart;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData.Carts;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Carts.CreateCart;

public class CreateCartHandlerTests
{
    private readonly ICartRepository _cartRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IProductRepository _productRepository;
    private readonly ISaleCounterRepository _saleCounterRepository;
    private readonly IMapper _mapper;
    private readonly CreateCartHandler _handler;

    public CreateCartHandlerTests()
    {
        _cartRepository = Substitute.For<ICartRepository>();
        _branchRepository = Substitute.For<IBranchRepository>();
        _productRepository = Substitute.For<IProductRepository>();
        _saleCounterRepository = Substitute.For<ISaleCounterRepository>();

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Cart, CartResult>();
            cfg.CreateMap<CartItem, CartItemCommand>();
        });

        _mapper = mapperConfig.CreateMapper();

        _handler = new CreateCartHandler(
            _cartRepository,
            _branchRepository,
            _productRepository,
            _saleCounterRepository,
            _mapper,
            Substitute.For<ILogger<CreateCartHandler>>());
    }

    [Fact(DisplayName = "Create cart successfully → returns CartResult")]
    public async Task Handle_ValidCommand_ReturnsCartResult()
    {
        // Given
        var command = CreateCartHandlerTestData.GenerateValidCommand();
        var branch = BranchTestData.GenerateValidBranch();
        var product = ProductTestData.GenerateValidProduct();
        var cart = CartTestData.GenerateValidCart();

        _branchRepository.GetByIdAsync(command.BranchId, Arg.Any<CancellationToken>())
            .Returns(branch);
        _productRepository.GetByIdAsync(command.Items.First().ProductId)
            .Returns(product);
        _saleCounterRepository.GetAndIncrementSaleNumberAsync(command.BranchId, Arg.Any<CancellationToken>())
            .Returns(1);
        _cartRepository.CreateAsync(Arg.Any<Cart>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(cart));
        _cartRepository.SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(true));

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.CustomerId.Should().Be(command.CustomerId);
        result.BranchId.Should().Be(command.BranchId);
        result.Items.Should().HaveCount(command.Items.Count);
        result.Items.First().ProductId.Should().Be(command.Items.First().ProductId);

        await _branchRepository.Received(1).GetByIdAsync(command.BranchId, Arg.Any<CancellationToken>());
        await _productRepository.Received(1).GetByIdAsync(command.Items.First().ProductId);
        await _saleCounterRepository.Received(1).GetAndIncrementSaleNumberAsync(command.BranchId, Arg.Any<CancellationToken>());
        await _cartRepository.Received(1).CreateAsync(Arg.Any<Cart>(), Arg.Any<CancellationToken>());
        await _cartRepository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Branch not found → throws EntityNotFoundException")]
    public async Task Handle_BranchNotFound_ThrowsEntityNotFoundException()
    {
        // Given
        var command = CreateCartHandlerTestData.GenerateValidCommand();
        _branchRepository.GetByIdAsync(command.BranchId, Arg.Any<CancellationToken>())
            .Returns((Branch?)null);

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
        var command = CreateCartHandlerTestData.GenerateValidCommand();
        var branch = BranchTestData.GenerateValidBranch();

        _branchRepository.GetByIdAsync(command.BranchId, Arg.Any<CancellationToken>())
            .Returns(branch);
        _productRepository.GetByIdAsync(command.Items.First().ProductId)
            .Returns((Product?)null);

        // When
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should()
            .ThrowAsync<EntityNotFoundException>();
    }
}
