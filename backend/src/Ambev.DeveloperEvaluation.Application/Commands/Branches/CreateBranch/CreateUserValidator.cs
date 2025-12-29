using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Commands.Branches.CreateBranch;

/// <summary>
/// Validator for CreateBranchCommand that defines validation rules for branch creation command.
/// </summary>
public class CreateBranchValidator : AbstractValidator<CreateBranchCommand>
{
    /// <summary>
    /// Initializes a new instance of the CreateBranchCommandValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// - Name: Required, must be between 3 and 100 characters
    /// - Code: Required, must be between 3 and 20 characters
    /// - City: Required, must be between 2 and 50 characters
    /// - State: Required, must be a valid Brazilian state abbreviation
    /// </remarks>
    public CreateBranchValidator()
    {
        RuleFor(branch => branch.Name).NotEmpty().Length(3, 100);
        RuleFor(branch => branch.Code).NotEmpty().Length(3, 20);
        RuleFor(branch => branch.City).NotEmpty().Length(2, 50);
        RuleFor(branch => branch.State).NotEmpty().Length(2);
    }
}