using Ambev.DeveloperEvaluation.Application.Branches.GetBranch;
using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Branches.GetBranch;

/// <summary>
/// Unit tests for <see cref="GetBranchValidator"/>.
/// Ensures that commands are validated according to the rules:
/// Id must be not empty.
/// </summary>
public class GetBranchValidatorTests
{
    private readonly GetBranchValidator _validator;

    public GetBranchValidatorTests()
    {
        _validator = new GetBranchValidator();
    }

    [Fact(DisplayName = "Valid command should pass validation")]
    public void Should_PassValidation_ForValidCommand()
    {
        // Given
        var command = new GetBranchCommand(Guid.NewGuid());

        // When
        var result = _validator.TestValidate(command);

        // Then
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact(DisplayName = "Empty Id should fail validation")]
    public void Should_FailValidation_WhenIdIsEmpty()
    {
        // Given
        var command = new GetBranchCommand(Guid.Empty);

        // When
        var result = _validator.TestValidate(command);

        // Then
        result.ShouldHaveValidationErrorFor(c => c.Id)
            .WithErrorMessage("Branch ID is required");
    }
}
