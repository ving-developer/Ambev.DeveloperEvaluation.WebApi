using Ambev.DeveloperEvaluation.Application.Auth.AuthenticateUser;
using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Auth.AuthenticateUser
{
    /// <summary>
    /// Unit tests for <see cref="AuthenticateUserValidator"/>.
    /// Ensures that commands are validated according to the rules:
    /// Email must be not empty and a valid email address,
    /// Password must be not empty and at least 6 characters long.
    /// </summary>
    public class AuthenticateUserValidatorTests
    {
        private readonly AuthenticateUserValidator _validator;

        public AuthenticateUserValidatorTests()
        {
            _validator = new AuthenticateUserValidator();
        }

        [Fact(DisplayName = "Valid command should pass validation")]
        public void Should_PassValidation_ForValidCommand()
        {
            // Given
            var command = new AuthenticateUserCommand
            {
                Email = "test@example.com",
                Password = "Password123"
            };

            // When
            var result = _validator.TestValidate(command);

            // Then
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory(DisplayName = "Invalid emails should fail validation")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("invalid-email")]
        public void Should_FailValidation_ForInvalidEmail(string? email)
        {
            // Given
            var command = new AuthenticateUserCommand
            {
                Email = email!,
                Password = "Password123"
            };

            // When
            var result = _validator.TestValidate(command);

            // Then
            result.ShouldHaveValidationErrorFor(c => c.Email);
        }

        [Theory(DisplayName = "Invalid passwords should fail validation")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("12345")] // less than 6 chars
        public void Should_FailValidation_ForInvalidPassword(string? password)
        {
            // Given
            var command = new AuthenticateUserCommand
            {
                Email = "test@example.com",
                Password = password!
            };

            // When
            var result = _validator.TestValidate(command);

            // Then
            result.ShouldHaveValidationErrorFor(c => c.Password);
        }
    }
}
