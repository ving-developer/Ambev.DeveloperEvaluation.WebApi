using Ambev.DeveloperEvaluation.Application.Common.Users;
using Ambev.DeveloperEvaluation.Application.Queries.Users.GetUserById;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData.Users;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Users.GetUserById;

/// <summary>
/// Contains unit tests for the <see cref="GetUserByIdHandler"/> class.
/// </summary>
public class GetUserByIdHandlerTests
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly GetUserByIdHandler _handler;

    public GetUserByIdHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetUserByIdHandler(_userRepository, _mapper, Substitute.For<ILogger<GetUserByIdHandler>>());
    }

    [Fact(DisplayName = "Given valid user ID When getting user Then returns user result")]
    public async Task Handle_ValidRequest_ReturnsUserResult()
    {
        // Given
        var command = GetUserHandlerTestData.GenerateValidCommand();
        var user = UserTestData.GenerateValidUser();
        user.Id = command.Id;

        var expectedResult = new UserResult
        {
            Id = user.Id,
            Name = user.Username,
            Email = user.Email,
            Phone = user.Phone,
            Role = user.Role,
            Status = user.Status
        };

        _userRepository.GetByIdAsync(user.Id, Arg.Any<CancellationToken>())
            .Returns(user);
        _mapper.Map<UserResult>(user).Returns(expectedResult);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Id.Should().Be(user.Id);
        result.Name.Should().Be(user.Username);
        result.Email.Should().Be(user.Email);
        await _userRepository.Received(1).GetByIdAsync(user.Id, Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<UserResult>(user);
    }

    [Fact(DisplayName = "Given non-existing user ID When getting user Then throws EntityNotFoundException")]
    public async Task Handle_NonExistingUser_ThrowsEntityNotFoundException()
    {
        // Given
        var command = GetUserHandlerTestData.GenerateValidCommand();

        _userRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns((User)null!);

        // When
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<EntityNotFoundException>();

        await _userRepository.Received(1).GetByIdAsync(command.Id, Arg.Any<CancellationToken>());
    }
}
