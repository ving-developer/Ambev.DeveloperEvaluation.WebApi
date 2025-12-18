using Ambev.DeveloperEvaluation.Application.Common.Users;
using Ambev.DeveloperEvaluation.Application.Queries.Users.SearchUsers;
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

namespace Ambev.DeveloperEvaluation.Unit.Application.Users.SearchUsers;

public class SearchUsersHandlerTests
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly SearchUsersHandler _handler;

    public SearchUsersHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();

        var mapperConfig = new MapperConfiguration(cfg => cfg.CreateMap<User, UserResult>());

        _mapper = mapperConfig.CreateMapper();

        _handler = new SearchUsersHandler(
            _userRepository,
            _mapper,
            Substitute.For<ILogger<SearchUsersHandler>>());
    }

    [Fact(DisplayName = "Handle valid request → returns paginated users")]
    public async Task Handle_ValidRequest_ReturnsPaginatedResponse()
    {
        // Given
        var users = new List<User>
        {
            UserTestData.GenerateValidUser(),
            UserTestData.GenerateValidUser()
        };

        var paginatedResult = new PaginatedResponse<User>(
            users,
            currentPage: 1,
            totalPages: 1,
            totalCount: 2
        );

        _userRepository
            .GetPaginatedAsync(
                Arg.Any<Expression<Func<User, bool>>>(),
                1,
                10,
                Arg.Any<string>(),
                Arg.Any<CancellationToken>())
            .Returns(paginatedResult);

        var command = new SearchUsersQuery
        {
            Page = 1,
            PageSize = 10,
            OrderBy = "username"
        };

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Data.Should().HaveCount(2);
        result.TotalCount.Should().Be(2);

        await _userRepository.Received(1).GetPaginatedAsync(
            Arg.Any<Expression<Func<User, bool>>>(),
            command.Page,
            command.PageSize,
            command.OrderBy,
            Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Handle request returning empty result → returns empty paginated response")]
    public async Task Handle_EmptyList_ReturnsEmptyPaginatedResponse()
    {
        // Given
        var empty = new PaginatedResponse<User>(
            [],
            currentPage: 1,
            totalPages: 0,
            totalCount: 0);

        _userRepository
            .GetPaginatedAsync(
                Arg.Any<Expression<Func<User, bool>>>(),
                1,
                10,
                Arg.Any<string>(),
                Arg.Any<CancellationToken>())
            .Returns(empty);

        var command = new SearchUsersQuery
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

        await _userRepository.Received(1).GetPaginatedAsync(
            Arg.Any<Expression<Func<User, bool>>>(),
            1,
            10,
            Arg.Any<string>(),
            Arg.Any<CancellationToken>());
    }
}
