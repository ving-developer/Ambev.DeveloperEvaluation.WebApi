using Ambev.DeveloperEvaluation.Application.Auth.AuthenticateUser;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData.Auth;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Auth.AuthenticateUser;

/// <summary>
/// Contains unit tests for the <see cref="AuthenticateUserHandler"/> class.
/// </summary>
public class AuthenticateUserHandlerTests
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly AuthenticateUserHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthenticateUserHandlerTests"/> class.
    /// Sets up all dependencies and the handler under test using NSubstitute mocks.
    /// </summary>
    public AuthenticateUserHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _passwordHasher = Substitute.For<IPasswordHasher>();
        _jwtTokenGenerator = Substitute.For<IJwtTokenGenerator>();
        _handler = new AuthenticateUserHandler(_userRepository, _passwordHasher, _jwtTokenGenerator, Substitute.For<ILogger<AuthenticateUserHandler>>());
    }

    /// <summary>
    /// Tests that valid credentials result in a successful authentication.
    /// Verifies that:
    /// - A token is generated
    /// - The result contains the expected user email, username, and role
    /// </summary>
    [Fact(DisplayName = "Given valid credentials When authenticating user Then returns token and user info")]
    public async Task Handle_ValidCredentials_ReturnsAuthenticateUserResult()
    {
        // Given
        var user = UserTestData.GenerateValidUser();
        var command = AuthenticateUserHandlerTestData.GenerateValidCommand();
        command.Email = user.Email;
        command.Password = user.Password;

        _userRepository.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>()).Returns(user);
        _passwordHasher.VerifyPassword(command.Password, user.Password).Returns(true);
        _jwtTokenGenerator.GenerateToken(user).Returns("fake-jwt-token");

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Id.Should().Be(user.Id);
        result.Token.Should().Be("fake-jwt-token");
        result.Email.Should().Be(user.Email);
        result.Name.Should().Be(user.Username);
        result.Role.Should().Be(user.Role.ToString());
    }

    /// <summary>
    /// Tests that invalid credentials throw an <see cref="UnauthorizedAccessException"/>.
    /// Ensures that:
    /// - Users with incorrect passwords cannot authenticate
    /// - The exception message is "Invalid credentials"
    /// </summary>
    [Fact(DisplayName = "Given invalid credentials When authenticating user Then throws UnauthorizedAccessException")]
    public async Task Handle_InvalidCredentials_ThrowsUnauthorizedAccessException()
    {
        // Given
        var user = UserTestData.GenerateValidUser();
        var command = AuthenticateUserHandlerTestData.GenerateValidCommand();
        command.Email = user.Email;
        command.Password = "wrong-password";

        _userRepository.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>()).Returns(user);
        _passwordHasher.VerifyPassword(command.Password, user.Password).Returns(false);

        // When
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("Invalid credentials");
    }

    /// <summary>
    /// Tests that attempting to authenticate a non-existing user throws an <see cref="UnauthorizedAccessException"/>.
    /// Verifies that:
    /// - Users not present in the repository cannot authenticate
    /// - The exception message is "Invalid credentials"
    /// </summary>
    [Fact(DisplayName = "Given non-existing user When authenticating Then throws UnauthorizedAccessException")]
    public async Task Handle_NonExistingUser_ThrowsUnauthorizedAccessException()
    {
        // Given
        var command = AuthenticateUserHandlerTestData.GenerateValidCommand();

        _userRepository.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>()).Returns((User)null!);

        // When
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("Invalid credentials");
    }

    /// <summary>
    /// Tests that attempting to authenticate an inactive user throws an <see cref="UnauthorizedAccessException"/>.
    /// Ensures that:
    /// - Users with <see cref="UserStatus.Suspended"/> cannot authenticate
    /// - The exception message is "User is not active"
    /// </summary>
    [Fact(DisplayName = "Given inactive user When authenticating Then throws UnauthorizedAccessException")]
    public async Task Handle_InactiveUser_ThrowsUnauthorizedAccessException()
    {
        // Given
        var user = UserTestData.GenerateSuspendedUser();

        var command = AuthenticateUserHandlerTestData.GenerateValidCommand();
        command.Email = user.Email;
        command.Password = user.Password;

        _userRepository.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>()).Returns(user);
        _passwordHasher.VerifyPassword(command.Password, user.Password).Returns(true);

        // When
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("User is not active");
    }
}
