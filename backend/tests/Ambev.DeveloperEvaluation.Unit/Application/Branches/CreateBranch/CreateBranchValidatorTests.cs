using Ambev.DeveloperEvaluation.Application.Commands.Branches.CreateBranch;
using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Branches.CreateBranch;

/// <summary>
/// Unit tests for <see cref="CreateBranchValidator"/>.
/// Ensures that branch creation commands are validated according to the rules:
/// - Name: Required, 3-100 characters
/// - Code: Required, 3-20 characters
/// - City: Required, 2-50 characters
/// - State: Required, 2 characters (Brazilian state abbreviation)
/// </summary>
public class CreateBranchValidatorTests
{
    private readonly CreateBranchValidator _validator;

    public CreateBranchValidatorTests()
    {
        _validator = new CreateBranchValidator();
    }

    [Fact(DisplayName = "Valid command should pass validation")]
    public void Should_PassValidation_ForValidCommand()
    {
        // Given
        var command = new CreateBranchCommand
        {
            Name = "Branch Name",
            Code = "BR001",
            City = "São Paulo",
            State = "SP"
        };

        // When
        var result = _validator.TestValidate(command);

        // Then
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory(DisplayName = "Invalid Code should fail validation")]
    [InlineData("")]
    [InlineData("AB")]
    [InlineData("3a08864c-cf99-438b-a54d-9508ffe05d20")]
    public void Should_FailValidation_ForInvalidCode(string code)
    {
        var command = new CreateBranchCommand
        {
            Name = "Branch Name",
            Code = code,
            City = "São Paulo",
            State = "SP"
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Code);
    }

    [Theory(DisplayName = "Invalid City should fail validation")]
    [InlineData("")]
    [InlineData("A")]
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
    public void Should_FailValidation_ForInvalidCity(string city)
    {
        var command = new CreateBranchCommand
        {
            Name = "Branch Name",
            Code = "BR001",
            City = city,
            State = "SP"
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.City);
    }

    [Theory(DisplayName = "Invalid State should fail validation")]
    [InlineData("")]
    [InlineData("S")]
    [InlineData("SPS")]
    public void Should_FailValidation_ForInvalidState(string state)
    {
        var command = new CreateBranchCommand
        {
            Name = "Branch Name",
            Code = "BR001",
            City = "São Paulo",
            State = state
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.State);
    }
}
