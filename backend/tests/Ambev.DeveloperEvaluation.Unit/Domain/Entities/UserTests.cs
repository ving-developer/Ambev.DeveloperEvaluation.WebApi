using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Unit tests for the User entity.
/// Covers creation, validation, and status change scenarios.
/// </summary>
public class UserTests
{
    #region Creation Tests

    [Fact(DisplayName = "Creating user with valid data should succeed")]
    public void Given_ValidData_When_CreatingUser_Then_ShouldCreateSuccessfully()
    {
        // Guiven
        var user = UserTestData.GenerateValidUser();

        // When & Then
        user.Username.Should().NotBeNullOrWhiteSpace();
        user.Email.Should().Contain("@");
        user.Phone.Should().MatchRegex(@"^\+?\d{10,15}$");
        user.Role.Should().NotBe(UserRole.None);
        user.Status.Should().NotBe(UserStatus.Unknown);
    }

    #endregion

    #region Username Tests

    [Theory(DisplayName = "SetUsername should throw DomainException for invalid usernames")]
    [InlineData("")]
    [InlineData("ab")]
    [InlineData("thisusernameiswaytoolongtobevalidandshouldfailthisusernameiswaytoolongtobevalidandshouldfail")]
    [InlineData("invalid username!")]
    public void Given_InvalidUsername_When_SetUsername_Then_ShouldThrow(string invalidUsername)
    {
        var user = UserTestData.GenerateValidUser();

        Action act = () => user.SetUsername(invalidUsername);

        act.Should().Throw<DomainException>()
            .WithMessage("*Username*");
    }

    #endregion

    #region Email Tests

    [Theory(DisplayName = "SetEmail should throw DomainException for invalid emails")]
    [InlineData("")]
    [InlineData("not-an-email")]
    [InlineData("invalid@")]
    public void Given_InvalidEmail_When_SetEmail_Then_ShouldThrow(string invalidEmail)
    {
        var user = UserTestData.GenerateValidUser();

        Action act = () => user.SetEmail(invalidEmail);

        act.Should().Throw<DomainException>()
            .WithMessage("*Email*");
    }

    #endregion

    #region Phone Tests

    [Theory(DisplayName = "SetPhone should throw DomainException for invalid phones")]
    [InlineData("")]
    [InlineData("12345")]
    [InlineData("+55119")]
    public void Given_InvalidPhone_When_SetPhone_Then_ShouldThrow(string invalidPhone)
    {
        var user = UserTestData.GenerateValidUser();

        Action act = () => user.SetPhone(invalidPhone);

        act.Should().Throw<DomainException>()
            .WithMessage("*Phone*");
    }

    #endregion

    #region Role & Status Tests

    [Fact(DisplayName = "SetRole should throw DomainException for invalid role")]
    public void Given_InvalidRole_When_SetRole_Then_ShouldThrow()
    {
        var user = UserTestData.GenerateValidUser();

        Action act = () => user.SetRole((UserRole)999);

        act.Should().Throw<DomainException>()
            .WithMessage("*role*");
    }

    [Fact(DisplayName = "SetStatus should throw DomainException for invalid status")]
    public void Given_InvalidStatus_When_SetStatus_Then_ShouldThrow()
    {
        var user = UserTestData.GenerateValidUser();

        Action act = () => user.SetStatus((UserStatus)999);

        act.Should().Throw<DomainException>()
            .WithMessage("*status*");
    }

    #endregion

    #region Status Change Methods

    [Fact(DisplayName = "User status should change to Active when activated")]
    public void Given_SuspendedUser_When_Activated_Then_StatusShouldBeActive()
    {
        var user = UserTestData.GenerateValidUser();
        user.SetStatus(UserStatus.Suspended);

        user.Activate();

        user.Status.Should().Be(UserStatus.Active);
    }

    [Fact(DisplayName = "User status should change to Inactive when deactivated")]
    public void Given_ActiveUser_When_Deactivated_Then_StatusShouldBeInactive()
    {
        var user = UserTestData.GenerateValidUser();
        user.SetStatus(UserStatus.Active);

        user.Deactivate();

        user.Status.Should().Be(UserStatus.Inactive);
    }

    [Fact(DisplayName = "User status should change to Suspended when suspended")]
    public void Given_ActiveUser_When_Suspended_Then_StatusShouldBeSuspended()
    {
        var user = UserTestData.GenerateValidUser();
        user.SetStatus(UserStatus.Active);

        user.Suspend();

        user.Status.Should().Be(UserStatus.Suspended);
    }

    #endregion
}
