using Ambev.DeveloperEvaluation.Domain.Enums;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Commands.Products.CreateProduct;

/// <summary>
/// Validator for CreateProductCommand that defines validation rules for product creation.
/// </summary>
public class CreateProductValidator : AbstractValidator<CreateProductCommand>
{
    /// <summary>
    /// Initializes a new instance of the CreateProductCommandValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// - Title: Required, between 1 and 100 characters
    /// - Description: Optional, maximum 1000 characters
    /// - Category: Required, between 1 and 50 characters
    /// - Price: Must be greater than 0
    /// - Image: Optional, maximum 255 characters
    /// - Rating: Rate between 0 and 5, Count >= 0
    /// </remarks>
    public CreateProductValidator()
    {
        RuleFor(p => p.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");

        RuleFor(p => p.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");

        RuleFor(p => p.Category)
            .NotEmpty().WithMessage("Category is required.")
            .Must(c => c != ProductCategory.Unknown)
            .WithMessage("Category cannot be 'Other'.");

        RuleFor(p => p.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero.");

        RuleFor(p => p.Image)
            .MaximumLength(255).WithMessage("Image URL cannot exceed 255 characters.");

        RuleFor(p => p.Rating)
            .NotNull().WithMessage("Rating must be provided.");

        RuleFor(p => p.Rating.Rate)
            .InclusiveBetween(0, 5).WithMessage("Rating rate must be between 0 and 5.");

        RuleFor(p => p.Rating.Count)
            .GreaterThanOrEqualTo(0).WithMessage("Rating count cannot be negative.");
    }
}
