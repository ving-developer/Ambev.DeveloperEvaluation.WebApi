using Ambev.DeveloperEvaluation.Application.Users.UpdateUser;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Unit.Application.TestData.Users;
using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Users.UpdateUser;

public class UpdateUserValidatorTests
{
    private readonly UpdateUserValidator _validator;

    public UpdateUserValidatorTests()
    {
        _validator = new UpdateUserValidator();
    }

    [Fact(DisplayName = "Invalid command should fail validation")]
    public void Should_FailValidation_ForInvalidCommand()
    {
        // Given
        var command = UpdateUserHandlerTestData.GenerateInvalidCommand();

        // When
        var result = _validator.TestValidate(command);

        // Then
        result.ShouldHaveValidationErrorFor(c => c.Id);
        result.ShouldHaveValidationErrorFor(c => c.Username);
        result.ShouldHaveValidationErrorFor(c => c.Phone);
        result.ShouldHaveValidationErrorFor(c => c.Email);
        result.ShouldHaveValidationErrorFor(c => c.Status);
        result.ShouldHaveValidationErrorFor(c => c.Role);
    }

    [Theory(DisplayName = "Invalid emails should fail validation")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("invalid-email")]
    public void Should_FailValidation_ForInvalidEmail(string? email)
    {
        var command = UpdateUserHandlerTestData.GenerateValidCommand() with { Email = email! };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Email);
    }

    [Theory(DisplayName = "Invalid status should fail validation")]
    [InlineData(UserStatus.Unknown)]
    public void Should_FailValidation_ForInvalidStatus(UserStatus status)
    {
        var command = UpdateUserHandlerTestData.GenerateValidCommand() with { Status = status };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Status);
    }

    [Theory(DisplayName = "Invalid role should fail validation")]
    [InlineData(UserRole.None)]
    public void Should_FailValidation_ForInvalidRole(UserRole role)
    {
        var command = UpdateUserHandlerTestData.GenerateValidCommand() with { Role = role };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Role);
    }
}
