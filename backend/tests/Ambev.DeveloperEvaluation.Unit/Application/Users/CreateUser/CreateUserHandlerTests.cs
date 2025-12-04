using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Application.Users.Common;
using Ambev.DeveloperEvaluation.Application.Users.CreateUser;
using Ambev.DeveloperEvaluation.Application.Users.UpdateUser;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData.Users;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Users.CreateUser;

/// <summary>
/// Contains unit tests for the <see cref="CreateUserHandler"/> class.
/// </summary>
public class CreateUserHandlerTests
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher _passwordHasher;
    private readonly CreateUserHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateUserHandlerTests"/> class.
    /// Sets up the test dependencies and creates fake data generators.
    /// </summary>
    public CreateUserHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _mapper = Substitute.For<IMapper>();
        _passwordHasher = Substitute.For<IPasswordHasher>();
        _handler = new CreateUserHandler(_userRepository, _mapper, _passwordHasher, Substitute.For<ILogger<CreateUserHandler>>());
    }

    /// <summary>
    /// Tests that a valid user creation request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid user data When creating user Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var command = CreateUserHandlerTestData.GenerateValidCommand();
        var user = new User(
            Guid.NewGuid(),
            command.Username,
            command.Email,
            command.Phone,
            command.Password,
            command.Role,
            command.Status);

        var result = UserResultTestData.GenerateFromUser(user);

        _mapper.Map<User>(command).Returns(user);
        _mapper.Map<UserResult>(user).Returns(result);

        _userRepository.CreateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>())
            .Returns(user);
        _passwordHasher.HashPassword(Arg.Any<string>()).Returns("hashedPassword");

        // When
        var createUserResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        createUserResult.Should().NotBeNull();
        createUserResult.Id.Should().Be(user.Id);
        await _userRepository.Received(1).CreateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that the password is hashed before saving the user.
    /// </summary>
    [Fact(DisplayName = "Given user creation request When handling Then password is hashed")]
    public async Task Handle_ValidRequest_HashesPassword()
    {
        // Given
        var command = CreateUserHandlerTestData.GenerateValidCommand();
        var originalPassword = command.Password;
        const string hashedPassword = "h@shedPassw0rd";

        _passwordHasher.HashPassword(originalPassword).Returns(hashedPassword);
        _userRepository.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>())
            .Returns((User)null!);

        var userWithHashedPassword = new User(
            Guid.NewGuid(),
            command.Username,
            command.Email,
            command.Phone,
            hashedPassword,
            command.Role,
            command.Status);

        _mapper.Map<User>(command).Returns(userWithHashedPassword);

        _userRepository.CreateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>())
            .Returns(userWithHashedPassword);

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        _passwordHasher.Received(1).HashPassword(originalPassword);

        _mapper.Received(1).Map<User>(
            Arg.Is<CreateUserCommand>(c => c.Password == hashedPassword));

        await _userRepository.Received(1).CreateAsync(
            Arg.Is<User>(u => u.Password == hashedPassword),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that the mapper is called with the correct command.
    /// </summary>
    [Fact(DisplayName = "Given valid command When handling Then maps command to user entity")]
    public async Task Handle_ValidRequest_MapsCommandToUser()
    {
        // Given
        var command = CreateUserHandlerTestData.GenerateValidCommand();
        var user = new User(
            Guid.NewGuid(),
            command.Username,
            command.Email,
            command.Phone,
            command.Password,
            command.Role,
            command.Status);

        _mapper.Map<User>(command).Returns(user);
        _userRepository.CreateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>())
            .Returns(user);
        _passwordHasher.HashPassword(Arg.Any<string>()).Returns("hashedPassword");

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        _mapper.Received(1).Map<User>(Arg.Is<CreateUserCommand>(c =>
            c.Username == command.Username &&
            c.Email == command.Email &&
            c.Phone == command.Phone &&
            c.Status == command.Status &&
            c.Role == command.Role));
    }

    /// <summary>
    /// Tests when the request is valid, but the email field already exists.
    /// </summary>
    [Fact(DisplayName = "Given The user's email address is repeated Then throws DomainException")]
    public async Task Returns_DomainException_When_EmailAlreadyExists()
    {
        // Guiven
        var command = CreateUserHandlerTestData.GenerateValidCommand();

        var existingUser = UserTestData.GenerateValidUser();
        existingUser.SetEmail(command.Email);

        _userRepository.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>())
            .Returns(existingUser);

        // When
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<DomainException>()
            .WithMessage($"User with email {command.Email} already exists");

        await _userRepository.DidNotReceive().CreateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
    }
}
