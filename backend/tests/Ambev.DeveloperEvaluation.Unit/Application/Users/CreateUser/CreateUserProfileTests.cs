using Ambev.DeveloperEvaluation.Application.Commands.Users.CreateUser;
using Ambev.DeveloperEvaluation.Application.Common.Users;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Application.TestData.Users;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using AutoMapper;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Users.CreateUser;

/// <summary>
/// Unit tests for <see cref="CreateUserProfile"/> AutoMapper configuration.
/// </summary>
public class CreateUserProfileTests
{
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes AutoMapper with the CreateUserProfile for testing.
    /// </summary>
    public CreateUserProfileTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<CreateUserProfile>();
        });
        _mapper = config.CreateMapper();
        config.AssertConfigurationIsValid(); // Ensures mapping configuration is valid
    }

    /// <summary>
    /// Tests mapping from CreateUserCommand to User.
    /// </summary>
    [Fact(DisplayName = "CreateUserCommand to User mapping should map all properties correctly")]
    public void CreateUserCommand_To_User_Should_Map_Correctly()
    {
        // Given
        var command = CreateUserHandlerTestData.GenerateValidCommand();

        // When
        var user = _mapper.Map<User>(command);

        // Then
        user.Should().NotBeNull();
        user.Username.Should().Be(command.Username);
        user.Password.Should().Be(command.Password);
        user.Email.Should().Be(command.Email);
        user.Phone.Should().Be(command.Phone);
        user.Status.Should().Be(command.Status);
        user.Role.Should().Be(command.Role);
    }

    /// <summary>
    /// Tests mapping from User to CreateUserResult.
    /// </summary>
    [Fact(DisplayName = "User to CreateUserResult mapping should map Name property correctly")]
    public void User_To_CreateUserResult_Should_Map_Correctly()
    {
        // Given
        var user = UserTestData.GenerateValidUser();

        // When
        var result = _mapper.Map<UserResult>(user);

        // Then
        result.Should().NotBeNull();
        result.Id.Should().Be(user.Id);
        result.Name.Should().Be(user.Username);
    }
}
