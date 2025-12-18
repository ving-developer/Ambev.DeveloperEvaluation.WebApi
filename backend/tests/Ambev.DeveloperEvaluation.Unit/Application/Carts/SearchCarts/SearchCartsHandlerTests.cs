using Ambev.DeveloperEvaluation.Application.Common.Carts;
using Ambev.DeveloperEvaluation.Application.Queries.Carts.SearchCarts;
using Ambev.DeveloperEvaluation.Common.Pagination;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Linq.Expressions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Carts.SearchCarts;

/// <summary>
/// Unit tests for <see cref="SearchCartsHandler"/>
/// </summary>
public class SearchCartsHandlerTests
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;
    private readonly SearchCartsHandler _handler;

    public SearchCartsHandlerTests()
    {
        _cartRepository = Substitute.For<ICartRepository>();

        var mapperConfig = new MapperConfiguration(cfg => cfg.CreateMap<Cart, CartResult>());

        _mapper = mapperConfig.CreateMapper();

        _handler = new SearchCartsHandler(
            _cartRepository,
            _mapper,
            Substitute.For<ILogger<SearchCartsHandler>>());
    }

    [Fact(DisplayName = "Handle valid request → returns paginated carts")]
    public async Task Handle_ValidRequest_ReturnsPaginatedResponse()
    {
        // Given
        var carts = CartTestData.GenerateList(3);

        var paginatedResult = new PaginatedResponse<Cart>(
            carts,
            currentPage: 1,
            totalPages: 1,
            totalCount: carts.Count
        );

        _cartRepository
            .GetPaginatedAsync(
                Arg.Any<Expression<System.Func<Cart, bool>>>(),
                1,
                10,
                Arg.Any<string>(),
                Arg.Any<CancellationToken>())
            .Returns(paginatedResult);

        var command = new SearchCartsQuery
        {
            Page = 1,
            PageSize = 10,
            OrderBy = "SaleDate"
        };

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Data.Should().HaveCount(3);
        result.TotalCount.Should().Be(3);

        await _cartRepository.Received(1).GetPaginatedAsync(
            Arg.Any<Expression<System.Func<Cart, bool>>>(),
            command.Page,
            command.PageSize,
            command.OrderBy,
            Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Handle request returning empty result → returns empty paginated response")]
    public async Task Handle_EmptyList_ReturnsEmptyPaginatedResponse()
    {
        // Given
        var empty = new PaginatedResponse<Cart>(
            CartTestData.GenerateList(0),
            currentPage: 1,
            totalPages: 0,
            totalCount: 0
        );

        _cartRepository
            .GetPaginatedAsync(
                Arg.Any<Expression<System.Func<Cart, bool>>>(),
                1,
                10,
                Arg.Any<string>(),
                Arg.Any<CancellationToken>())
            .Returns(empty);

        var command = new SearchCartsQuery
        {
            Page = 1,
            PageSize = 10
        };

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Data.Should().BeEmpty();
        result.TotalCount.Should().Be(0);

        await _cartRepository.Received(1).GetPaginatedAsync(
            Arg.Any<Expression<System.Func<Cart, bool>>>(),
            1,
            10,
            Arg.Any<string>(),
            Arg.Any<CancellationToken>());
    }
}
