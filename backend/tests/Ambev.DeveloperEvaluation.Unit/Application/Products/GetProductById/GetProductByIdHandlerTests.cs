using Ambev.DeveloperEvaluation.Application.Common.Products;
using Ambev.DeveloperEvaluation.Application.Queries.Products.GetProductById;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products.GetProductById;

/// <summary>
/// Unit tests for <see cref="GetProductByIdHandler"/>
/// </summary>
public class GetProductByIdHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly GetProductByIdHandler _handler;

    public GetProductByIdHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Product, ProductResult>()
               .ForMember(dest => dest.Rating,
                          opt => opt.MapFrom(src => src.Rating == null
                              ? null
                              : new ProductRating { Rate = src.Rating.Rate, Count = src.Rating.Count }));
        });

        _mapper = mapperConfig.CreateMapper();
        _handler = new GetProductByIdHandler(
            _productRepository,
            _mapper,
            Substitute.For<ILogger<GetProductByIdHandler>>());
    }

    [Fact(DisplayName = "Get product successfully → returns ProductResult")]
    public async Task Handle_ValidCommand_ReturnsProductResult()
    {
        // Given
        var product = ProductTestData.GenerateValidProduct();
        product.Id = Guid.NewGuid();

        var command = new GetProductByIdQuery(product.Id);

        _productRepository.GetByIdAsync(product.Id, Arg.Any<CancellationToken>(),
            Arg.Any<System.Linq.Expressions.Expression<Func<Product, object>>>()).Returns(product);


        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Id.Should().Be(product.Id);
        result.Title.Should().Be(product.Title);
        result.Price.Should().Be(product.Price);

        await _productRepository.Received(1).GetByIdAsync(
                product.Id,
                Arg.Any<CancellationToken>(),
                Arg.Any<System.Linq.Expressions.Expression<Func<Product, object>>>());
    }

    [Fact(DisplayName = "Product not found → throws EntityNotFoundException")]
    public async Task Handle_ProductNotFound_ThrowsEntityNotFoundException()
    {
        // Given
        var command = new GetProductByIdQuery(Guid.NewGuid());

        _productRepository.GetByIdAsync(
                command.Id,
                Arg.Any<CancellationToken>(),
                Arg.Any<System.Linq.Expressions.Expression<Func<Product, object>>>()
            )
            .Returns((Product?)null);

        // When
        Func<Task> act = async () =>
            await _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should()
            .ThrowAsync<EntityNotFoundException>();

        await _productRepository.Received(1)
            .GetByIdAsync(
                command.Id,
                Arg.Any<CancellationToken>(),
                Arg.Any<System.Linq.Expressions.Expression<Func<Product, object>>>()
            );
    }
}
