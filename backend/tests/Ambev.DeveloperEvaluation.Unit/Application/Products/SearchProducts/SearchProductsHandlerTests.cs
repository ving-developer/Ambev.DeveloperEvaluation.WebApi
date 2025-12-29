using Ambev.DeveloperEvaluation.Application.Common.Products;
using Ambev.DeveloperEvaluation.Application.Queries.Products.SearchProducts;
using Ambev.DeveloperEvaluation.Common.Pagination;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData.Products;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Linq.Expressions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products.SearchProducts;

/// <summary>
/// Unit tests for <see cref="SearchProductsHandler"/>
/// </summary>
public class SearchProductsHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly SearchProductsHandler _handler;

    public SearchProductsHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new SearchProductsHandler(
            _productRepository,
            _mapper,
            Substitute.For<ILogger<SearchProductsHandler>>());
    }

    [Fact(DisplayName = "Given valid request When listing products Then returns paginated result")]
    public async Task Handle_ValidRequest_ReturnsPaginatedResult()
    {
        // Given
        var command = ListProductsHandlerTestData.GenerateValidCommand();
        var products = ProductTestData.GenerateList(3);
        var paginated = new PaginatedResponse<Product>(
            products,
            currentPage: 1,
            totalPages: 1,
            totalCount: 3
        );

        var mappedList = products.ConvertAll(p => new ProductResult
        {
            Id = p.Id,
            Title = p.Title,
            Description = p.Description,
            Price = p.Price,
            Category = p.Category,
            Image = p.Image,
            Rating = new ProductRating() { Rate = p.Rating?.Rate ?? 0m, Count = p.Rating?.Count ?? 0 }
        });

        _productRepository.GetPaginatedAsync(
            Arg.Any<Expression<Func<Product, bool>>>(),
            command.Page,
            command.PageSize,
            command.OrderBy,
            Arg.Any<CancellationToken>())
        .Returns(paginated);

        _mapper.Map<List<ProductResult>>(products).Returns(mappedList);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Data.Should().HaveCount(3);
        result.CurrentPage.Should().Be(1);
        result.TotalPages.Should().Be(1);
        result.TotalCount.Should().Be(3);

        await _productRepository.Received(1).GetPaginatedAsync(
            Arg.Any<Expression<Func<Product, bool>>>(),
            command.Page,
            command.PageSize,
            command.OrderBy,
            Arg.Any<CancellationToken>());

        _mapper.Received(1).Map<List<ProductResult>>(products);
    }

    [Fact(DisplayName = "Given empty database When listing products Then returns empty result")]
    public async Task Handle_NoProducts_ReturnsEmptyList()
    {
        // Given
        var command = ListProductsHandlerTestData.GenerateValidCommand();

        var paginated = new PaginatedResponse<Product>(
            new List<Product>(),
            currentPage: 1,
            totalPages: 0,
            totalCount: 0
        );

        _productRepository.GetPaginatedAsync(
            Arg.Any<Expression<Func<Product, bool>>>(),
            command.Page,
            command.PageSize,
            command.OrderBy,
            Arg.Any<CancellationToken>())
        .Returns(paginated);

        _mapper.Map<List<ProductResult>>(Arg.Any<List<Product>>()).Returns(new List<ProductResult>());

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Data.Should().BeEmpty();
        result.TotalCount.Should().Be(0);
        result.TotalPages.Should().Be(0);

        await _productRepository.Received(1).GetPaginatedAsync(
            Arg.Any<Expression<Func<Product, bool>>>(),
            command.Page,
            command.PageSize,
            command.OrderBy,
            Arg.Any<CancellationToken>());
    }
}
