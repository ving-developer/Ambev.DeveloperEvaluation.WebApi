using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Queries.Product.GetProductById;

/// <summary>
/// Validator for GetProductCommand
/// </summary>
public class GetProductValidator : AbstractValidator<GetProductCommand>
{
    /// <summary>
    /// Initializes validation rules for GetProductCommand
    /// </summary>
    public GetProductValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Product ID is required");
    }
}
