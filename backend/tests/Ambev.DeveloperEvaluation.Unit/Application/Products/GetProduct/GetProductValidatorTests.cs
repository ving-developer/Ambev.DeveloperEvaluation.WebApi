using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products.GetProduct;

/// <summary>
/// Unit tests for <see cref="GetProductValidator"/>.
/// Ensures that commands are validated according to the rules:
/// Id must be not empty.
/// </summary>
public class GetProductValidatorTests
{
    private readonly GetProductValidator _validator;

    public GetProductValidatorTests()
    {
        _validator = new GetProductValidator();
    }

    [Fact(DisplayName = "Valid command should pass validation")]
    public void Should_PassValidation_ForValidCommand()
    {
        // Given
        var command = new GetProductCommand(Guid.NewGuid());

        // When
        var result = _validator.TestValidate(command);

        // Then
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact(DisplayName = "Empty Id should fail validation")]
    public void Should_FailValidation_WhenIdIsEmpty()
    {
        // Given
        var command = new GetProductCommand(Guid.Empty);

        // When
        var result = _validator.TestValidate(command);

        // Then
        result.ShouldHaveValidationErrorFor(c => c.Id)
            .WithErrorMessage("Product ID is required");
    }
}
