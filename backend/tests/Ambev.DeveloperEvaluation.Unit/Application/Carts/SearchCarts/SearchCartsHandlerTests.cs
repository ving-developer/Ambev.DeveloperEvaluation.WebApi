using Ambev.DeveloperEvaluation.Application.Common.Carts;
using Ambev.DeveloperEvaluation.Application.Queries.Carts.SearchCarts;
using Ambev.DeveloperEvaluation.Common.Pagination;
using Ambev.DeveloperEvaluation.Domain.Queries.Carts;
using Ambev.DeveloperEvaluation.Domain.ReadModels.Carts;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Carts.SearchCarts;

/// <summary>
/// Unit tests for <see cref="SearchCartsHandler"/>
/// </summary>
public class SearchCartsHandlerTests
{
    private readonly ICartQuery _cartQuery;
    private readonly IMapper _mapper;
    private readonly SearchCartsHandler _handler;

    public SearchCartsHandlerTests()
    {
        _cartQuery = Substitute.For<ICartQuery>();

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<CartReadModel, CartResult>();
            cfg.CreateMap<SearchCartsQuery, CartSearchFilter>()
                .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.OrderBy));
        });

        _mapper = mapperConfig.CreateMapper();

        _handler = new SearchCartsHandler(
            _cartQuery,
            _mapper,
            Substitute.For<ILogger<SearchCartsHandler>>());
    }

    [Fact(DisplayName = "Handle valid request → returns paginated carts")]
    public async Task Handle_ValidRequest_ReturnsPaginatedResponse()
    {
        // Given
        var carts = new List<CartReadModel>
        {
            new () { Id = Guid.NewGuid(), SaleNumber = "S001" },
            new () { Id = Guid.NewGuid(), SaleNumber = "S002" },
            new () { Id = Guid.NewGuid(), SaleNumber = "S003" }
        };

        _cartQuery
            .SearchAsync(Arg.Any<CartSearchFilter>(), Arg.Any<CancellationToken>())
            .Returns(new PaginatedResponse<CartReadModel>(carts, 1, 1, carts.Count));

        var command = new SearchCartsQuery
        {
            Page = 1,
            PageSize = 10
        };

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Data.Should().HaveCount(3);
        result.TotalCount.Should().Be(3);

        await _cartQuery.Received(1).SearchAsync(
            Arg.Any<CartSearchFilter>(), Arg.Any<CancellationToken>());
    }
}