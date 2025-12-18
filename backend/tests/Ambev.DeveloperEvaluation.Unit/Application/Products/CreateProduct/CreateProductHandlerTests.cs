using Ambev.DeveloperEvaluation.Application.Commands.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Application.Common.Products;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData.Products;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products.CreateProduct;

public class CreateProductHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly CreateProductHandler _handler;

    public CreateProductHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<ProductRating, Rating>().ReverseMap();
            cfg.CreateMap<CreateProductCommand, Product>();
            cfg.CreateMap<Product, ProductResult>()
               .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating));
        });

        _mapper = mapperConfig.CreateMapper();

        _handler = new CreateProductHandler(
            _productRepository,
            _mapper,
            Substitute.For<ILogger<CreateProductHandler>>());
    }

    [Fact(DisplayName = "Create product successfully → returns ProductResult")]
    public async Task Handle_ValidCommand_ReturnsCreatedProduct()
    {
        // Given
        var command = CreateProductHandlerTestData.GenerateValidCommand();

        var product = _mapper.Map<Product>(command);
        var generatedId = Guid.NewGuid();

        _productRepository
            .CreateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>())
            .Returns(callInfo =>
            {
                var product = callInfo.ArgAt<Product>(0);
                product.Id = generatedId;
                return Task.FromResult(product);
            });

        _productRepository.SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(true));

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Id.Should().Be(generatedId);
        result.Title.Should().Be(command.Title);
        result.Description.Should().Be(command.Description);
        result.Category.Should().Be(command.Category);
        result.Price.Should().Be(command.Price);
        result.Image.Should().Be(command.Image);
        result.Rating.Rate.Should().Be(command.Rating.Rate);
        result.Rating.Count.Should().Be(command.Rating.Count);

        await _productRepository.Received(1).CreateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>());
        await _productRepository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}